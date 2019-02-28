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
    using static ONEbook.SpreadSheets.Column;

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
                .SetData(reportData, grandTotal, message.CustLevel.IsSuperLevel())
                .SetRowMappers(GetRowMapper(message, false), GetRowMapper(message, true), GetFooterRowMapper())
                .UseTemplate(GetTemplateInfo(message))
                .Header(ReportResources.ResourceManager.GetString)
                .Sheet("Sheet 1")
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message, param), result);
        }

        internal Mapper<ReportModel> GetRowMapper(ReportExcelQuery message, bool isTotal)
        {
            var mapper = new Mapper<ReportModel>()
                .Map(A, data => isTotal ? ReportResources.Total : data.Username)
                .Map(B, data => data.Super)
                .Map(C, data => data.SuperIncome)
                .Map(D, data => data.SuperResult)
                .Map(E, data => data.SuperStock)
                .Map(F, data => data.Wagers)
                .Map(G, data => data.Stake)
                .Map(H, data => data.ValidStake)
                .Map(I, data => data.MemberIncome)
                .Map(J, data => data.Agent)
                .Map(K, data => data.AgentResult)
                .Map(L, data => data.Master);

            return mapper;
        }

        internal Mapper<ReportModel> GetFooterRowMapper()
            => new Mapper<ReportModel>()
                .Map(A, data => ReportResources.ProfitMagin)
                .Map(B, data => data.SuperResult / data.SuperStock);

        private TemplateInfo GetTemplateInfo(ReportExcelQuery message)
            => new TemplateInfo(
                GetExcelTemplatePath(message.IsOwner ? message.CustLevel.DisplayName + "Owner" : message.CustLevel.DisplayName),
                noInfomationText: SharedResources.NoInformationIsAvailable,
                titleStartIndex: -1,
                headerStartIndex: 1,
                bodyStartIndex: 2);

        private string GetExcelFileName(ReportExcelQuery message, ReportParam param)
           => string.Join(
               "_",
               param.FromDate.ToShortDate(),
               param.ToDate.ToShortDate());

        protected internal override string GetExcelTemplateFolder() => "Reports";
    }
}