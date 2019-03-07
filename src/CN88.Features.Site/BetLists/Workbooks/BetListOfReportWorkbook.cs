namespace CN88.Features.Site.BetLists.Workbooks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Features.BetLists.Models;
    using Features.BetLists.Resources;
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

        BetListOfReportWorkbook SetData(BetListOfReportViewModel comboParlayDetails);
    }

    public class BetListOfReportWorkbookImpl : NpoiWorkbook<IEnumerable<TicketDto>>, BetListOfReportWorkbook
    {
        private int currentRowIndex;

        public BetListOfReportViewModel BetDataViewModel { get; private set; }

        public BetListOfReportWorkbook SetData(BetListOfReportViewModel betDataViewModel)
        {
            BetDataViewModel = betDataViewModel;

            return this;
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
            row.SetCellValue(7, BetListsResources.AgentPercent);

            if (BetDataViewModel.CustLevel.IsMasterOrSuperLevel())
            {
                row.SetCellValue(8, BetListsResources.MAPercent);
            }

            if (BetDataViewModel.CustLevel.IsSuperLevel())
            {
                row.SetCellValue(9, BetListsResources.SMAPercent);
                row.SetCellValue(10, BetListsResources.CoPercent);
            }
        }

        protected override void FillBody(Sheet sheet)
        {
            currentRowIndex = TemplateInfo.BodyStartIndex;

            var row = sheet.GetRow(currentRowIndex);

            if (!BetDataViewModel.BetListData.Tickets.Any())
            {
                FillNoInfoRow(sheet);
                return;
            }

            row.SetCellValue(0, BetListsResources.Total);
            row.SetCellValue(3, BetDataViewModel.BetListData.Total);
            row.SetCellValue(4, BetDataViewModel.BetListTotal.TotalStake);
            row.SetCellValue(5, BetDataViewModel.BetListTotal.TotalWinLoss);
            currentRowIndex++;

            row = sheet.GetRow(currentRowIndex);
            row.SetCellValue(0, BetListsResources.WinLossWithCommission);
            row.SetCellValue(5, BetDataViewModel.BetListTotal.WinLossWithCommission - 11);
            currentRowIndex++;
        }
    }
}