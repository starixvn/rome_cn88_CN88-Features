namespace CN88.Features.Site.Reports.Handlers
{
    using System;
    using Features.Reports.AppResources;
    using Features.Reports.Models;
    using Features.Reports.Params;
    using Features.Reports.Services;
    using Features.Shared.AppResources;
    using Features.Shared.Extensions;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Requests;
    using Shared.ViewModels;

    public class ReportsExcelHandler : ReportsBaseHandler, IRequestHandler<ReportExcelQuery, ExcelFileViewModel>
    {
        private readonly ReportService reportService;
        private readonly ReportsWorkbook workbook;

        public ReportsExcelHandler(
            ReportService reportService,
            ReportsWorkbook workbook)
        {
            this.reportService = reportService;
            this.workbook = workbook;
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

            var result = workbook
                .UseTemplate(GetTemplateInfo(message))
                .Header(ReportResources.ResourceManager.GetString)
                .Body(reportData, GetRowMapper(message))
                .Sheet("Sheet 1")
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message, param), result);
        }

        internal Mapper<ReportModel> GetRowMapper(ReportExcelQuery message)
        {
            var mapper = new Mapper<ReportModel>();

            return mapper;
        }

        private TemplateInfo GetTemplateInfo(ReportExcelQuery message)
            => new TemplateInfo(
                GetExcelTemplatePath(message.CustLevel.DisplayName),
                noInfomationText: SharedResources.NoInformationIsAvailable);

        private string GetExcelFileName(ReportExcelQuery message, ReportParam param)
           => string.Join(
               "_",
               param.FromDate.ToShortDate(),
               param.ToDate.ToShortDate());
    }
}