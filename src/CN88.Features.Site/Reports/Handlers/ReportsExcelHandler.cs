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
                .SetRowMappers(GetRowMapper(message, grandTotal, false), GetRowMapper(message, grandTotal, true), GetFooterRowMapper())
                .UseTemplate(GetTemplateInfo(message))
                .Header(ReportResources.ResourceManager.GetString)
                .Sheet("Sheet 1")
                .ExportToBytes();

            return new ExcelFileViewModel(GetExcelFileName(message, param), result);
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
                return GetAgentRowMapper(message, grandTotal, isTotal);
            }

            return GetMemberRowMapper(isTotal);
        }

        internal Mapper<ReportModel> GetSuperRowMapper(ReportExcelQuery message, ReportModel grandTotal, bool isTotal)
        {
            var mapper = new Mapper<ReportModel>()
                .Map(A, data => isTotal ? ReportResources.Total : data.Username)
                .Map(B, data => data.Super)
                .Map(C, data => data.SuperIncome)
                .Map(D, data => data.SuperResult)
                .Map(E, data => data.SuperStock);

            if (message.IsOwner)
            {
                mapper.Map(F, data => data.Wagers)
                    .Map(G, data => data.Stake)
                    .Map(H, data => data.ValidStake)
                    .Map(I, data => data.MemberIncome)
                    .Map(J, data => data.Agent)
                    .Map(K, data => data.AgentResult)
                    .Map(L, data => data.Master);
            }
            else
            {
                mapper.Map(F, data => isTotal ? 100 : CalculatePercentage(data.SuperStock, grandTotal.SuperStock))
                    .Map(G, data => isTotal ? string.Empty : data.HeadlinePoss)
                    .Map(H, data => data.HeadlineIncome)
                    .Map(I, data => data.HeadlineResult)
                    .Map(J, data => data.HeadlineStock)
                    .Map(K, data => isTotal ? 100 : CalculatePercentage(data.HeadlineStock, grandTotal.HeadlineStock))
                    .Map(L, data => data.Wagers)
                    .Map(M, data => data.Stake)
                    .Map(N, data => data.ValidStake)
                    .Map(O, data => data.MemberIncome)
                    .Map(P, data => data.Agent)
                    .Map(Q, data => data.AgentResult)
                    .Map(R, data => data.Master);
            }

            return mapper;
        }

        internal Mapper<ReportModel> GetMasterRowMapper(ReportExcelQuery message, ReportModel grandTotal, bool isTotal)
        {
            var mapper = new Mapper<ReportModel>()
                .Map(A, data => isTotal ? ReportResources.Total : data.Username)
                .Map(B, data => isTotal ? string.Empty : data.Name)
                .Map(C, data => data.MasterIncome)
                .Map(D, data => data.MasterResult)
                .Map(E, data => data.MasterStock)
                .Map(F, data => isTotal ? 100 : CalculatePercentage(data.MasterStock, grandTotal.MasterStock));

            if (message.IsOwner)
            {
                mapper.Map(G, data => data.Wagers)
                    .Map(H, data => data.Stake)
                    .Map(I, data => data.ValidStake)
                    .Map(J, data => data.MemberIncome)
                    .Map(K, data => data.Agent)
                    .Map(L, data => data.AgentResult);
            }
            else
            {
                mapper.Map(G, data => data.Super)
                    .Map(H, data => isTotal ? string.Empty : data.SuperPoss)
                    .Map(I, data => data.SuperIncome)
                    .Map(J, data => data.SuperResult)
                    .Map(K, data => data.SuperStock)
                    .Map(L, data => isTotal ? 100 : CalculatePercentage(data.SuperStock, grandTotal.SuperStock))
                    .Map(M, data => data.Wagers)
                    .Map(N, data => data.Stake)
                    .Map(O, data => data.ValidStake)
                    .Map(P, data => data.MemberIncome)
                    .Map(Q, data => data.Agent)
                    .Map(R, data => data.AgentResult);
            }

            return mapper;
        }

        internal Mapper<ReportModel> GetAgentRowMapper(ReportExcelQuery message, ReportModel grandTotal, bool isTotal)
        {
            var mapper = new Mapper<ReportModel>()
                .Map(A, data => isTotal ? ReportResources.Total : data.Username)
                .Map(B, data => isTotal ? string.Empty : data.Name)
                .Map(C, data => data.Agent)
                .Map(D, data => data.AgentPoss)
                .Map(E, data => data.AgentIncome)
                .Map(F, data => data.AgentResult)
                .Map(G, data => data.AgentStock);

            if (message.IsOwner)
            {
                mapper.Map(H, data => data.Wagers)
                    .Map(I, data => data.Stake)
                    .Map(J, data => data.ValidStake)
                    .Map(K, data => data.MemberIncome)
                    .Map(L, data => data.AgentCurrency);
            }
            else
            {
                mapper.Map(H, data => isTotal ? string.Empty : data.MasterPoss)
                    .Map(I, data => data.MasterIncome)
                    .Map(J, data => data.MasterResult)
                    .Map(K, data => data.Wagers)
                    .Map(L, data => data.Stake)
                    .Map(M, data => data.ValidStake)
                    .Map(N, data => data.MemberIncome)
                    .Map(O, data => data.AgentCurrency);
            }

            return mapper;
        }

        internal Mapper<ReportModel> GetMemberRowMapper(bool isTotal)
        {
            var mapper = new Mapper<ReportModel>()
                .Map(A, data => isTotal ? ReportResources.Total : data.Username)
                .Map(B, data => isTotal ? string.Empty : data.Name)
                .Map(C, data => data.Wagers)
                .Map(D, data => data.Stake)
                .Map(E, data => data.ValidStake)
                .Map(F, data => data.MemberIncome)
                .Map(G, data => isTotal ? string.Empty : data.AgentPoss)
                .Map(H, data => data.AgentIncome)
                .Map(I, data => data.AgentResult);

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

        private string GetExcelFileName(ReportExcelQuery message, ReportParam param)
           => string.Join(
               "_",
               param.FromDate.ToShortDate(),
               param.ToDate.ToShortDate());

        protected internal override string GetExcelTemplateFolder() => "Reports";

        private decimal CalculatePercentage(decimal devidend, decimal devisor)
            => devisor == 0 ? 0 : devidend / devisor * 100;
    }
}