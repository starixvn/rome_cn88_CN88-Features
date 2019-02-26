namespace CN88.Features.Site.Reports.Handlers
{
    using System;
    using Features.Reports.Models;
    using Features.Reports.Services;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Requests;
    using Shared.ViewModels;

    public class ReportsExcelHandler : ReportsBaseHandler, IRequestHandler<ReportExcelQuery, ExcelFileViewModel>
    {
        private readonly Workbook<ReportModel> workbook;
        private readonly ReportService reportService;

        public ReportsExcelHandler(ReportService reportService)
        {
            this.reportService = reportService;
        }

        public ExcelFileViewModel Handle(ReportExcelQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var param = CreateReportParam(message);
            var reportData = reportService.GetReportData(param);
            var grandTotal = AccumulateGrandTotal(reportData);

            throw new NotImplementedException();
        }
    }
}