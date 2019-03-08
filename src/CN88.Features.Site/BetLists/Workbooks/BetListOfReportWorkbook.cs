namespace CN88.Features.Site.BetLists.Workbooks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BetList.Core.Builders;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Features.BetLists.Models;
    using Features.BetLists.Resources;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using ViewModels;

    public interface BetListOfReportWorkbook
    {
        IWorkbook Workbook { get; }

        Workbook<IEnumerable<TicketDto>> UseTemplate(TemplateInfo templateInfo);

        Workbook<IEnumerable<TicketDto>> Sheet(string sheetName);

        Workbook<IEnumerable<TicketDto>> Title(string titleText);

        Workbook<IEnumerable<TicketDto>> Header(Func<string, string> headerMapping);

        byte[] ExportToBytes();

        BetListOfReportWorkbook SetData(
            BetListOfReportViewModel comboParlayDetails,
            IBetListExcelBuilder excelBuilder,
            IBetListCN88ExcelBuilder cn88ExcelBuilder);

        void SetRTFHelper(RTFHelper rtfHelper);
    }

    public class BetListOfReportWorkbookImpl : NpoiWorkbook<IEnumerable<TicketDto>>, BetListOfReportWorkbook
    {
        private const int DataSheetIndex = 1;
        private int currentRowIndex;
        private RTFHelper rtfHelper;
        private IBetListExcelBuilder excelBuilder;
        private IBetListCN88ExcelBuilder cn88ExcelBuilder;

        public BetListOfReportViewModel BetDataViewModel { get; private set; }

        public Row TotalRowTemplate { get; private set; }

        public Row WinLossWithCommissionRowTemplate { get; private set; }

        public Row BodyRowTemplate { get; private set; }

        public BetListOfReportWorkbook SetData(
            BetListOfReportViewModel betDataViewModel,
            IBetListExcelBuilder excelBuilder,
            IBetListCN88ExcelBuilder cn88ExcelBuilder)
        {
            BetDataViewModel = betDataViewModel;
            this.excelBuilder = excelBuilder;
            this.cn88ExcelBuilder = cn88ExcelBuilder;

            return this;
        }

        public void SetRTFHelper(RTFHelper rtfHelper)
        {
            this.rtfHelper = rtfHelper;
        }

        protected override Sheet LoadTemplateSheet()
        {
            var sheet = GetSheetAt(TemplateInfo.SheetIndex);

            currentRowIndex = TemplateInfo.BodyStartIndex;
            TotalRowTemplate = sheet.GetRow(currentRowIndex++);
            WinLossWithCommissionRowTemplate = sheet.GetRow(currentRowIndex++);
            BodyRowTemplate = sheet.GetRow(currentRowIndex++);

            return GetSheetAt(DataSheetIndex);
        }

        protected override void FillHeader(Sheet sheet)
        {
            var row = sheet.GetRow(TemplateInfo.HeaderStartIndex);

            row.SetCellValue(0, BetListsResources.DateTime);
            row.SetCellValue(1, BetListsResources.Member + "/" + BetListsResources.Commission);
            row.SetCellValue(2, BetListsResources.BetID + "/" + BetListsResources.BetType);
            row.SetCellValue(3, BetListsResources.Details);
            row.SetCellValue(4, BetListsResources.Stake);
            row.SetCellValue(5, BetListsResources.WinLoss);
            row.SetCellValue(6, BetListsResources.Status);

            if (BetDataViewModel.CustLevel.IsAgentLevel())
            {
                row.SetCellValue(7, BetListsResources.AgentPercent);
            }

            if (BetDataViewModel.CustLevel.IsMasterLevel())
            {
                row.SetCellValue(7, BetListsResources.AgentPercent);
                row.SetCellValue(8, BetListsResources.MAPercent);
            }

            if (BetDataViewModel.CustLevel.IsSuperLevel() || BetDataViewModel.CustLevel.IsHeadline())
            {
                row.SetCellValue(7, BetListsResources.AgentPercent);
                row.SetCellValue(8, BetListsResources.MAPercent);
                row.SetCellValue(9, BetListsResources.SMAPercent);
                row.SetCellValue(10, BetListsResources.CoPercent);
            }
        }

        protected override void FillBody(Sheet sheet)
        {
            RemoveSheetAt(TemplateInfo.SheetIndex);

            sheet.OddRowTemplate = BodyRowTemplate;

            currentRowIndex = TemplateInfo.BodyStartIndex;

            var row = sheet.GetRow(currentRowIndex);

            var tickets = BetDataViewModel.BetListData.Tickets.ToList();

            if (!tickets.Any())
            {
                FillNoInfoRow(sheet);
                return;
            }

            row.CopyCellsFrom(TotalRowTemplate);
            row.SetCellValue(0, BetListsResources.Total);
            row.SetCellValue(3, BetDataViewModel.BetListData.Total);
            row.SetCellValue(4, BetDataViewModel.BetListTotal.TotalStake);
            row.SetCellValue(5, BetDataViewModel.BetListTotal.TotalWinLoss);
            currentRowIndex++;

            row = sheet.GetRow(currentRowIndex);
            row.CopyCellsFrom(WinLossWithCommissionRowTemplate);
            row.SetCellValue(0, BetListsResources.WinLossWithCommission);
            row.SetCellValue(5, BetDataViewModel.BetListTotal.WinLossWithCommission);
            currentRowIndex++;

            var comFont = Workbook.CreateFont();
            comFont.Color = HSSFColor.Yellow.Index;
            var palette = ((HSSFWorkbook)Workbook).GetCustomPalette();
            palette.SetColorAtIndex(HSSFColor.Yellow.Index, 243, 156, 18);

            for (int i = 0; i < tickets.Count; i++)
            {
                var colIndex = 0;
                var ticket = tickets[i];

                row = sheet.GetRow(currentRowIndex);
                row.CopyCellsFrom(BodyRowTemplate);
                row.SetCellValue(colIndex++, $"{ticket.TransDate.ToShortDateFormat()}\n{ticket.TransDate.ToLongTime24h()}");

                var comm = ticket.Comm.ToString("#,##0.00");
                var richString = new HSSFRichTextString($"{ticket.UserName}\n{ ticket.UGroup } { comm }");
                richString.ApplyFont(ticket.UserName.Length, ticket.UserName.Length + ticket.UGroup.Length + comm.Length + 2, comFont);
                row.SetCellValue(colIndex++, richString);

                var cellValue = cn88ExcelBuilder.CreateBetInfoBuilder(ticket.BetTypeId)
                      .RenderRTF(ticket, BetDataViewModel.TicketHelper, BetDataViewModel.TicketHelper.GetOddsTypeNameById, rtfHelper);
                row.SetCellValue(colIndex++, cellValue);

                cellValue = cn88ExcelBuilder.CreateChoiceBuilder(ticket.BetTypeId)
                      .RenderRTF(ticket, BetDataViewModel.TicketHelper, null, rtfHelper).FirstOrDefault();
                row.SetCellValue(colIndex++, cellValue);

                cellValue = excelBuilder.CreateStakeBuilder(ticket.BetTypeId).RenderRTF(ticket, rtfHelper);
                row.SetCellValue(colIndex++, cellValue);

                cellValue = BuildWinLoss(ticket, rtfHelper);
                row.SetCellValue(colIndex++, cellValue);

                ticket.IP = string.Empty;
                cellValue = excelBuilder.CreateStatusBuilder(ticket.BetTypeId)
                    .RenderRTF(ticket, BetDataViewModel.TicketHelper, null, BetDataViewModel.DisplayVNIP, rtfHelper);
                row.SetCellValue(colIndex++, cellValue);

                if (BetDataViewModel.CustLevel.IsAgentLevel())
                {
                    row.SetCellValue(colIndex++, (int)ticket.AgentPositionTaking);
                }

                if (BetDataViewModel.CustLevel.IsMasterLevel())
                {
                    row.SetCellValue(colIndex++, (int)ticket.AgentPositionTaking);
                    row.SetCellValue(colIndex++, (int)ticket.MasterPositionTaking);
                }

                if (BetDataViewModel.CustLevel.IsSuperLevel() || BetDataViewModel.CustLevel.IsHeadline())
                {
                    row.SetCellValue(colIndex++, (int)ticket.AgentPositionTaking);
                    row.SetCellValue(colIndex++, (int)ticket.MasterPositionTaking);
                    row.SetCellValue(colIndex++, (int)ticket.SuperPositionTaking);
                    row.SetCellValue(colIndex++, (int)BetDataViewModel.CompanyPositionTaking(ticket));
                }

                currentRowIndex++;
            }
        }

        public static IRichTextString BuildWinLoss(ITicket ticket, RTFHelper rtfHelper)
        {
            var winLossBlockViewModel = new WinLostBlockViewModel(ticket);
            var comm = winLossBlockViewModel.CommVisibility ? winLossBlockViewModel.Comm : string.Empty;

            var rtfNumber = new RTFNumber(rtfHelper.RTFRenderer, rtfHelper.PosFont, rtfHelper.NegFont);

            if (!string.IsNullOrEmpty(winLossBlockViewModel.WinLost))
            {
                rtfNumber.Render(winLossBlockViewModel.WinLost + "\n");
            }

            if (!string.IsNullOrEmpty(comm))
            {
                rtfNumber.Render(comm);
            }

            var winLossRtfText = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();

            return winLossRtfText;
        }
    }
}