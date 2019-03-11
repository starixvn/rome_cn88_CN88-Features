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
                .Map(A, data => isTotal ? ReportResources.Total : data.Username)
                .Map(B, data => data.Super / divisor)
                .Map(C, data => data.SuperIncome / divisor)
                .Map(D, data => data.SuperResult / divisor)
                .Map(E, data => data.SuperStock / divisor);

            if (message.IsOwner)
            {
                mapper.Map(F, data => data.Wagers)
                    .Map(G, data => data.Stake / divisor)
                    .Map(H, data => data.ValidStake / divisor)
                    .Map(I, data => data.MemberIncome / divisor)
                    .Map(J, data => data.Agent / divisor)
                    .Map(K, data => data.AgentResult / divisor)
                    .Map(L, data => data.Master / divisor);
            }
            else
            {
                mapper.Map(F, data => isTotal ? 100 : CalculatePercentage(data.SuperStock, grandTotal.SuperStock))
                    .Map(G, data => isTotal ? string.Empty : data.HeadlinePoss)
                    .Map(H, data => data.HeadlineIncome / divisor)
                    .Map(I, data => data.HeadlineResult / divisor)
                    .Map(J, data => data.HeadlineStock / divisor)
                    .Map(K, data => isTotal ? 100 : CalculatePercentage(data.HeadlineStock, grandTotal.HeadlineStock))
                    .Map(L, data => data.Wagers)
                    .Map(M, data => data.Stake / divisor)
                    .Map(N, data => data.ValidStake / divisor)
                    .Map(O, data => data.MemberIncome / divisor)
                    .Map(P, data => data.Agent / divisor)
                    .Map(Q, data => data.AgentResult / divisor)
                    .Map(R, data => data.Master / divisor);
            }

            return mapper;
        }

        internal Mapper<ReportModel> GetMasterRowMapper(ReportExcelQuery message, ReportModel grandTotal, bool isTotal)
        {
            var mapper = new Mapper<ReportModel>()
                .Map(A, data => isTotal ? ReportResources.Total : data.Username)
                .Map(B, data => isTotal ? string.Empty : data.Name)
                .Map(C, data => data.MasterIncome / divisor)
                .Map(D, data => data.MasterResult / divisor)
                .Map(E, data => data.MasterStock / divisor)
                .Map(F, data => isTotal ? 100 : CalculatePercentage(data.MasterStock, grandTotal.MasterStock));

            if (message.IsOwner)
            {
                mapper.Map(G, data => data.Wagers)
                    .Map(H, data => data.Stake / divisor)
                    .Map(I, data => data.ValidStake / divisor)
                    .Map(J, data => data.MemberIncome / divisor)
                    .Map(K, data => data.Agent / divisor)
                    .Map(L, data => data.AgentResult / divisor);
            }
            else
            {
                mapper.Map(G, data => data.Super)
                    .Map(H, data => isTotal ? string.Empty : data.SuperPoss)
                    .Map(I, data => data.SuperIncome / divisor)
                    .Map(J, data => data.SuperResult / divisor)
                    .Map(K, data => data.SuperStock / divisor)
                    .Map(L, data => isTotal ? 100 : CalculatePercentage(data.SuperStock, grandTotal.SuperStock))
                    .Map(M, data => data.Wagers)
                    .Map(N, data => data.Stake / divisor)
                    .Map(O, data => data.ValidStake / divisor)
                    .Map(P, data => data.MemberIncome / divisor)
                    .Map(Q, data => data.Agent / divisor)
                    .Map(R, data => data.AgentResult / divisor);
            }

            return mapper;
        }

        internal Mapper<ReportModel> GetAgentRowMapper(ReportExcelQuery message, bool isTotal)
        {
            var mapper = new Mapper<ReportModel>()
                .Map(A, data => isTotal ? ReportResources.Total : data.Username)
                .Map(B, data => isTotal ? string.Empty : data.Name)
                .Map(C, data => data.Agent / divisor)
                .Map(D, data => data.AgentPoss)
                .Map(E, data => data.AgentIncome / divisor)
                .Map(F, data => data.AgentResult / divisor)
                .Map(G, data => data.AgentStock / divisor);

            if (message.IsOwner)
            {
                mapper.Map(H, data => data.Wagers)
                    .Map(I, data => data.Stake / divisor)
                    .Map(J, data => data.ValidStake / divisor)
                    .Map(K, data => data.MemberIncome / divisor);
            }
            else
            {
                mapper.Map(H, data => isTotal ? string.Empty : data.MasterPoss)
                    .Map(I, data => data.MasterIncome / divisor)
                    .Map(J, data => data.MasterResult / divisor)
                    .Map(K, data => data.Wagers)
                    .Map(L, data => data.Stake / divisor)
                    .Map(M, data => data.ValidStake / divisor)
                    .Map(N, data => data.MemberIncome / divisor);
            }

            return mapper;
        }

        internal Mapper<ReportModel> GetMemberRowMapper(bool isTotal)
        {
            var mapper = new Mapper<ReportModel>()
                .Map(A, data => isTotal ? ReportResources.Total : data.Username)
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