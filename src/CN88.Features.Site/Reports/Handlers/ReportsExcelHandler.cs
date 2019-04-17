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
        private int divisor = 1;

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

            divisor = (message.ActorLevel.IsHeadline()) ? 100 : 1;

            var param = CreateReportParam(message);
            var reportData = reportService.GetReportData(param);
            var grandTotal = AccumulateGrandTotal(reportData);

            var result = workbook
                .SetData(reportData, grandTotal, message.CustLevel.IsSuperLevel())
                .SetRowMappers(GetRowMapper(message, grandTotal, false), GetRowMapper(message, grandTotal, true), GetFooterRowMapper())
                .UseTemplate(GetTemplateInfo(message))
                .Header(ReportResources.ResourceManager.GetString)
                .Sheet("Sheet 1")
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(param), result);
        }

        internal Mapper<ReportModel> GetRowMapper(ReportExcelQuery message, ReportModel grandTotal, bool isTotal)
        {
            if (message.CustLevel.IsSuperLevel())
            {
                return GetSuperRowMapper(message, grandTotal, isTotal);
            }

            if (message.CustLevel.IsMasterLevel())
            {
                return GetMasterRowMapper(message, grandTotal, isTotal);
            }

            if (message.CustLevel.IsAgentLevel())
            {
                return GetAgentRowMapper(message, isTotal);
            }

            return GetMemberRowMapper(isTotal);
        }

        internal Mapper<ReportModel> GetSuperRowMapper(ReportExcelQuery message, ReportModel grandTotal, bool isTotal)
        {
            var mapper = new Mapper<ReportModel>()
                .Map(A, data => isTotal ? ReportResources.Total.ToUpperInvariant() : data.Username)
                .Map(B, data => data.Wagers)
                .Map(C, data => data.Stake / divisor)
                .Map(D, data => data.ValidStake / divisor)
                .Map(E, data => data.MemberIncome / divisor)
                .Map(F, data => data.Agent / divisor)
                .Map(G, data => data.AgentResult / divisor)
                .Map(H, data => data.Master / divisor)
                .Map(I, data => data.Super / divisor)
                .Map(J, data => data.SuperIncome / divisor)
                .Map(K, data => data.SuperResult / divisor)
                .Map(L, data => data.SuperStock / divisor);

            if (!message.IsOwner)
            {
                mapper.Map(M, data => isTotal ? 100 : CalculatePercentage(data.SuperStock, grandTotal.SuperStock))
                    .Map(N, data => isTotal ? string.Empty : data.HeadlinePoss)
                    .Map(O, data => data.HeadlineIncome / divisor)
                    .Map(P, data => data.HeadlineResult / divisor)
                    .Map(Q, data => data.HeadlineStock / divisor)
                    .Map(R, data => isTotal ? 100 : CalculatePercentage(data.HeadlineStock, grandTotal.HeadlineStock));
            }

            return mapper;
        }

        internal Mapper<ReportModel> GetMasterRowMapper(ReportExcelQuery message, ReportModel grandTotal, bool isTotal)
        {
            var mapper = new Mapper<ReportModel>()
                .Map(A, data => isTotal ? ReportResources.Total.ToUpperInvariant() : data.Username)
                .Map(B, data => isTotal ? string.Empty : data.Name)
                .Map(C, data => data.Wagers)
                .Map(D, data => data.Stake / divisor)
                .Map(E, data => data.ValidStake / divisor)
                .Map(F, data => data.MemberIncome / divisor)
                .Map(G, data => data.Agent / divisor)
                .Map(H, data => data.AgentResult / divisor)
                .Map(I, data => data.MasterIncome / divisor)
                .Map(J, data => data.MasterResult / divisor);

            if (message.IsOwner)
            {
                mapper.Map(K, data => data.MasterStock / divisor)
                    .Map(L, data => isTotal ? 100 : CalculatePercentage(data.MasterStock, grandTotal.MasterStock));
            }
            else
            {
                mapper.Map(K, data => data.Super / divisor)
                    .Map(L, data => data.MasterStock / divisor)
                    .Map(M, data => isTotal ? 100 : CalculatePercentage(data.MasterStock, grandTotal.MasterStock))
                    .Map(N, data => isTotal ? string.Empty : data.SuperPoss)
                    .Map(O, data => data.SuperIncome / divisor)
                    .Map(P, data => data.SuperResult / divisor)
                    .Map(Q, data => data.SuperStock / divisor)
                    .Map(R, data => isTotal ? 100 : CalculatePercentage(data.SuperStock, grandTotal.SuperStock));
            }

            return mapper;
        }

        internal Mapper<ReportModel> GetAgentRowMapper(ReportExcelQuery message, bool isTotal)
        {
            var mapper = new Mapper<ReportModel>()
                .Map(A, data => isTotal ? ReportResources.Total.ToUpperInvariant() : data.Username)
                .Map(B, data => isTotal ? string.Empty : data.Name)
                .Map(C, data => data.Wagers)
                .Map(D, data => data.Stake / divisor)
                .Map(E, data => data.ValidStake / divisor)
                .Map(F, data => data.MemberIncome / divisor)
                .Map(G, data => data.Agent / divisor)
                .Map(H, data => data.AgentPoss)
                .Map(I, data => data.AgentIncome / divisor)
                .Map(J, data => data.AgentResult / divisor);

            if (message.IsOwner)
            {
                mapper.Map(K, data => data.AgentStock / divisor);
            }
            else
            {
                mapper.Map(K, data => isTotal ? string.Empty : data.MasterPoss)
                    .Map(L, data => data.MasterIncome / divisor)
                    .Map(M, data => data.MasterResult / divisor)
                    .Map(N, data => data.AgentStock / divisor);
            }

            return mapper;
        }

        internal Mapper<ReportModel> GetMemberRowMapper(bool isTotal)
        {
            var mapper = new Mapper<ReportModel>()
                .Map(A, data => isTotal ? ReportResources.Total.ToUpperInvariant() : data.Username)
                .Map(B, data => isTotal ? string.Empty : data.Name)
                .Map(C, data => data.Wagers)
                .Map(D, data => data.Stake / divisor)
                .Map(E, data => data.ValidStake / divisor)
                .Map(F, data => data.MemberIncome / divisor)
                .Map(G, data => isTotal ? string.Empty : data.AgentPoss)
                .Map(H, data => data.AgentIncome / divisor)
                .Map(I, data => data.AgentResult / divisor);

            return mapper;
        }

        internal Mapper<ReportModel> GetFooterRowMapper()
            => new Mapper<ReportModel>()
                .Map(A, data => ReportResources.ProfitMagin)
                .Map(B, data => CalculatePercentage(data.SuperResult, data.SuperStock).FormatNumber() + " %");

        private TemplateInfo GetTemplateInfo(ReportExcelQuery message)
            => new TemplateInfo(
                GetExcelTemplatePath(message.IsOwner ? message.CustLevel.DisplayName + "Owner" : message.CustLevel.DisplayName),
                noInfomationText: SharedResources.NoInformationIsAvailable,
                titleStartIndex: -1,
                headerStartIndex: 1,
                bodyStartIndex: 2);

        private string GetExcelFileName(ReportParam param)
           => string.Join(
               "_",
               param.FromDate.ToShortDate(),
               param.ToDate.ToShortDate());

        protected internal override string GetExcelTemplateFolder() => "Reports";

        private decimal CalculatePercentage(decimal devidend, decimal devisor)
            => devisor == 0 ? 0 : devidend / devisor * 100;
    }
}