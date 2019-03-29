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
            row.SetCellValue(1, BetListsResources.Member + "/\r\n" + BetListsResources.Commission);
            row.SetCellValue(2, BetListsResources.BetID + "/\r\n" + BetListsResources.BetType);
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

            var rtfStake = new RTFNumber(rtfHelper.RTFRenderer, rtfHelper.PosFont, rtfHelper.NegFont);
            rtfStake.Render(BetDataViewModel.BetListTotal.TotalStakeStr);
            row.SetCellValue(4, rtfHelper.RTFRenderer.Render());
            rtfHelper.RTFRenderer.Clear();

            var rtfWinLoss = new RTFNumber(rtfHelper.RTFRenderer, rtfHelper.PosFont, rtfHelper.NegFont);
            rtfWinLoss.Render(BetDataViewModel.BetListTotal.TotalWinLossStr);
            row.SetCellValue(5, rtfHelper.RTFRenderer.Render());
            rtfHelper.RTFRenderer.Clear();

            currentRowIndex++;

            row = sheet.GetRow(currentRowIndex);
            row.CopyCellsFrom(WinLossWithCommissionRowTemplate);
            row.SetCellValue(0, BetListsResources.WinLossWithCommission);

            var rtfWinLoss2 = new RTFNumber(rtfHelper.RTFRenderer, rtfHelper.PosFont, rtfHelper.NegFont);
            rtfWinLoss2.Render(BetDataViewModel.BetListTotal.TotalStr);
            row.SetCellValue(5, rtfHelper.RTFRenderer.Render());
            rtfHelper.RTFRenderer.Clear();
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

                var comm = (ticket.PlayerDiscount * 100).ToString("#,##0.00");
                var username = ticket.UserName ?? string.Empty;
                var richString = new HSSFRichTextString($"{username}\n{ ticket.UGroup.ToUpper() } { comm }");
                richString.ApplyFont(username.Length, username.Length + ticket.UGroup.Length + comm.Length + 2, comFont);
                row.SetCellValue(colIndex++, richString);

                var cellValue = cn88ExcelBuilder.CreateBetInfoBuilder(ticket.BetTypeId)
                      .RenderRTF(ticket, BetDataViewModel.TicketHelper, BetDataViewModel.TicketHelper.GetOddsTypeNameById, rtfHelper);
                row.SetCellValue(colIndex++, cellValue);

                cellValue = cn88ExcelBuilder.CreateChoiceBuilder(ticket.BetTypeId)
                      .RenderRTF(ticket, BetDataViewModel.TicketHelper, null, rtfHelper).FirstOrDefault();
                row.SetCellValue(colIndex++, cellValue);

                ticket.Stake /= BetDataViewModel.Divisor;
                row.SetCellValue(colIndex++, ticket.Stake.ToString("#,##0.00"));

                cellValue = BuildWinLoss(ticket, rtfHelper, BetDataViewModel.Divisor);
                row.SetCellValue(colIndex++, cellValue);

                ticket.IP = string.Empty;
                cellValue = excelBuilder.CreateStatusBuilder(ticket.BetTypeId)
                    .RenderRTF(ticket, BetDataViewModel.TicketHelper, null, BetDataViewModel.DisplayVNIP, rtfHelper);
                row.SetCellValue(colIndex++, cellValue);

                if (BetDataViewModel.CustLevel.IsAgentLevel())
                {
                    row.SetCellValue(colIndex++, (int)(ticket.AgentPositionTaking * 100));
                }

                if (BetDataViewModel.CustLevel.IsMasterLevel())
                {
                    row.SetCellValue(colIndex++, (int)(ticket.AgentPositionTaking * 100));
                    row.SetCellValue(colIndex++, (int)(ticket.MasterPositionTaking * 100));
                }

                if (BetDataViewModel.CustLevel.IsSuperLevel() || BetDataViewModel.CustLevel.IsHeadline())
                {
                    row.SetCellValue(colIndex++, (int)(ticket.AgentPositionTaking * 100));
                    row.SetCellValue(colIndex++, (int)(ticket.MasterPositionTaking * 100));
                    row.SetCellValue(colIndex++, (int)(ticket.SuperPositionTaking * 100));
                    row.SetCellValue(colIndex++, (int)(BetDataViewModel.CompanyPositionTaking(ticket) * 100));
                }

                currentRowIndex++;
            }
        }

        public static IRichTextString BuildWinLoss(ITicket ticket, RTFHelper rtfHelper, int divisor)
        {
            var winLossBlockViewModel = new WinLostBlockViewModel(ticket, divisor);
            var comm = winLossBlockViewModel.CommVisibility ? winLossBlockViewModel.Comm : string.Empty;

            var rtfNumber = new RTFNumber(rtfHelper.RTFRenderer, rtfHelper.PosFont, rtfHelper.NegFont);

            if (!string.IsNullOrEmpty(winLossBlockViewModel.WinLost))
            {
                rtfNumber.Render(winLossBlockViewModel.WinLost);
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