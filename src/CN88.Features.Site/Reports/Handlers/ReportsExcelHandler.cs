namespace CN88.Features.Site.Reports.Handlers
{
    using System;
    using Extensions;
    using Features.Reports.AppResources;
    using Features.Reports.Constants;
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

    public class ReportsExcelHandler :
        ReportsBaseHandler,
        IRequestHandler<ReportExcelQuery, ExcelFileViewModel>,
        IRequestHandler<OldReportExcelQuery, ExcelFileViewModel>
    {
        private readonly ReportService reportService;
        private readonly AccountingPeriodService accountingPeriodService;
        private readonly ReportsWorkbook workbook;
        private int divisor = 1;
        private int totalStockPercent;

        public ReportsExcelHandler(
            ReportService reportService,
            AccountingPeriodService accountingPeriodService,
            ReportsWorkbook workbook)
        {
            this.reportService = reportService;
            this.accountingPeriodService = accountingPeriodService;
            this.workbook = workbook;
        }

        public ExcelFileViewModel Handle(ReportExcelQuery message)
        {
            var lastPeriod = accountingPeriodService.GetLastPeriod();
            message.FromDate = message.FromDate < lastPeriod.FromDate ? lastPeriod.FromDate : message.FromDate;

            return ExportExcel(message);
        }

        public ExcelFileViewModel Handle(OldReportExcelQuery message)
            => ExportExcel(message);

        private ExcelFileViewModel ExportExcel(ReportExcelBaseQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            divisor = message.Divisor;
            totalStockPercent = message.ReportType == ReportTypeConstants.SettledWager ? 100 : 0;
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

        internal Mapper<ReportModel> GetRowMapper(ReportExcelBaseQuery message, ReportModel grandTotal, bool isTotal)
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

        internal Mapper<ReportModel> GetSuperRowMapper(ReportExcelBaseQuery message, ReportModel grandTotal, bool isTotal)
        {
            var mapper = new Mapper<ReportModel>()
                .Map(A, data => isTotal ? ReportResources.Total.ToUpperInvariant() : data.Username)
                .Map(B, data => data.Wagers)
                .Map(C, data => CalculateNumber(data.Stake))
                .Map(D, data => CalculateNumber(data.ValidStake))
                .Map(E, data => RoundNumber(data.MemberIncome))
                .Map(F, data => RoundNumber(data.Agent))
                .Map(G, data => RoundNumber(data.AgentResult))
                .Map(H, data => RoundNumber(data.Master))
                .Map(I, data => RoundNumber(data.Super))
                .Map(J, data => RoundNumber(data.SuperIncome))
                .Map(K, data => RoundNumber(data.SuperResult))
                .Map(L, data => CalculateNumber(data.SuperStock));

            if (!message.IsOwner)
            {
                mapper.Map(M, data => isTotal ? totalStockPercent : CalculatePercentage(data.SuperStock, grandTotal.SuperStock))
                    .Map(N, data => isTotal ? string.Empty : data.HeadlinePoss)
                    .Map(O, data => RoundNumber(data.HeadlineIncome))
                    .Map(P, data => RoundNumber(data.HeadlineResult))
                    .Map(Q, data => CalculateNumber(data.HeadlineStock))
                    .Map(R, data => isTotal ? totalStockPercent : CalculatePercentage(data.HeadlineStock, grandTotal.HeadlineStock));
            }

            return mapper;
        }

        internal Mapper<ReportModel> GetMasterRowMapper(ReportExcelBaseQuery message, ReportModel grandTotal, bool isTotal)
        {
            var mapper = new Mapper<ReportModel>()
                .Map(A, data => isTotal ? ReportResources.Total.ToUpperInvariant() : data.Username)
                .Map(B, data => isTotal ? string.Empty : data.Name)
                .Map(C, data => data.Wagers)
                .Map(D, data => CalculateNumber(data.Stake))
                .Map(E, data => CalculateNumber(data.ValidStake))
                .Map(F, data => RoundNumber(data.MemberIncome))
                .Map(G, data => RoundNumber(data.Agent))
                .Map(H, data => RoundNumber(data.AgentResult))
                .Map(I, data => RoundNumber(data.MasterIncome))
                .Map(J, data => RoundNumber(data.MasterResult));

            if (message.IsOwner)
            {
                mapper.Map(K, data => CalculateNumber(data.MasterStock))
                    .Map(L, data => isTotal ? totalStockPercent : CalculatePercentage(data.MasterStock, grandTotal.MasterStock));
            }
            else
            {
                mapper.Map(K, data => RoundNumber(data.Super))
                    .Map(L, data => CalculateNumber(data.MasterStock))
                    .Map(M, data => isTotal ? totalStockPercent : CalculatePercentage(data.MasterStock, grandTotal.MasterStock))
                    .Map(N, data => isTotal ? string.Empty : data.SuperPoss)
                    .Map(O, data => RoundNumber(data.SuperIncome))
                    .Map(P, data => RoundNumber(data.SuperResult))
                    .Map(Q, data => CalculateNumber(data.SuperStock))
                    .Map(R, data => isTotal ? totalStockPercent : CalculatePercentage(data.SuperStock, grandTotal.SuperStock));
            }

            return mapper;
        }

        internal Mapper<ReportModel> GetAgentRowMapper(ReportExcelBaseQuery message, bool isTotal)
        {
            var mapper = new Mapper<ReportModel>()
                .Map(A, data => isTotal ? ReportResources.Total.ToUpperInvariant() : data.Username)
                .Map(B, data => isTotal ? string.Empty : data.Name)
                .Map(C, data => data.Wagers)
                .Map(D, data => CalculateNumber(data.Stake))
                .Map(E, data => CalculateNumber(data.ValidStake))
                .Map(F, data => RoundNumber(data.MemberIncome))
                .Map(G, data => RoundNumber(data.Agent))
                .Map(H, data => isTotal ? string.Empty : data.AgentPoss)
                .Map(I, data => RoundNumber(data.AgentIncome))
                .Map(J, data => RoundNumber(data.AgentResult));

            mapper.Map(message.IsOwner ? K : N, data => CalculateNumber(data.AgentStock));

            return mapper;
        }

        internal Mapper<ReportModel> GetMemberRowMapper(bool isTotal)
        {
            var mapper = new Mapper<ReportModel>()
                .Map(A, data => isTotal ? ReportResources.Total.ToUpperInvariant() : data.Username)
                .Map(B, data => isTotal ? string.Empty : data.Name)
                .Map(C, data => data.Wagers)
                .Map(D, data => CalculateNumber(data.Stake))
                .Map(E, data => CalculateNumber(data.ValidStake))
                .Map(F, data => RoundNumber(data.MemberIncome))
                .Map(G, data => isTotal ? string.Empty : data.AgentPoss)
                .Map(H, data => RoundNumber(data.AgentIncome))
                .Map(I, data => RoundNumber(data.AgentResult));

            return mapper;
        }

        internal static Mapper<ReportModel> GetFooterRowMapper()
            => new Mapper<ReportModel>()
                .Map(A, data => ReportResources.ProfitMargin)
                .Map(B, data => decimal.Round(data.SuperStock != 0 ? data.SuperResult / data.SuperStock : 0, 4));

        private TemplateInfo GetTemplateInfo(ReportExcelBaseQuery message)
            => new TemplateInfo(
                GetExcelTemplatePath(message.IsOwner ? message.CustLevel.DisplayName + "Owner" : message.CustLevel.DisplayName),
                noInfomationText: SharedResources.NoInformationIsAvailable,
                titleStartIndex: -1,
                headerStartIndex: 1,
                bodyStartIndex: 2);

        private static string GetExcelFileName(ReportParam param)
           => string.Join(
               "_",
               param.FromDate.ToShortDate(),
               param.ToDate.ToShortDate());

        protected internal override string GetExcelTemplateFolder() => "Reports";

        private static decimal CalculatePercentage(decimal devidend, decimal devisor)
            => devisor == 0 ? 0 : devidend / devisor * 100;

        private decimal RoundNumber(decimal number)
            => CalculateNumber(number).Round();

        private decimal CalculateNumber(decimal number)
            => number / divisor;
    }
}