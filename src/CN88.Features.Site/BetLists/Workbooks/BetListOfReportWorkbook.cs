namespace CN88.Features.Site.BetLists.Workbooks
{
    using System;
    using System.Collections.Generic;
    using Features.BetLists.Models;
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
        public BetListOfReportViewModel BetDataViewModel { get; private set; }

        public BetListOfReportWorkbook SetData(BetListOfReportViewModel betDataViewModel)
        {
            BetDataViewModel = betDataViewModel;

            return this;
        }
    }
}