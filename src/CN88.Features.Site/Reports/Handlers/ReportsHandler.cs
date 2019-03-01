namespace CN88.Features.Site.Reports.Handlers
{
    using Features.Reports.Services;
    using Features.Shared.Services;
    using MediatR;
    using Requests;
    using ViewModels;

    public class ReportsHandler : ReportsBaseHandler,
        IRequestHandler<IndexQuery, IndexViewModel>,
        IRequestHandler<ReportQuery, ReportViewModel>,
        IRequestHandler<PTHistoryQuery, PTHistoryViewModel>
    {
        private readonly SportGroupService sportService;
        private readonly BettypeGroupService betTypeService;
        private readonly AccountingPeriodService accountingPeriodService;
        private readonly SettlementSummaryService settlementSummaryService;
        private readonly ReportService reportService;

        public ReportsHandler(
            SportGroupService sportService,
            BettypeGroupService betTypeService,
            AccountingPeriodService accountingPeriodService,
            SettlementSummaryService settlementSummaryService,
            ReportService reportService)
        {
            this.sportService = sportService;
            this.betTypeService = betTypeService;
            this.accountingPeriodService = accountingPeriodService;
            this.settlementSummaryService = settlementSummaryService;
            this.reportService = reportService;
        }

        public IndexViewModel Handle(IndexQuery message)
        {
            var sportGroups = sportService.GetSportGroups();
            var betTypes = betTypeService.GetBetTypeGroupsBySport();
            var accountingPeriodGroups = accountingPeriodService.GetAccountingPeriodGroups();
            var currentPeriod = accountingPeriodService.GetCurrentPeriod();
            var lastPeriod = accountingPeriodService.GetLastPeriod();
            var settlementSummary = settlementSummaryService.GetSettlementSummaryReport();

            return new IndexViewModel(
                message,
                BuildSportGroupSelectListItems(sportGroups, 0),
                BuildBetTypeSelectListItems(betTypes, 0),
                accountingPeriodGroups,
                currentPeriod,
                lastPeriod,
                settlementSummary);
        }

        public ReportViewModel Handle(ReportQuery message)
        {
            var sportGroups = sportService.GetSportGroups();
            var betTypes = betTypeService.GetBetTypeGroupsBySport(message.UserSelectedSportGroupId);
            var lastPeriod = accountingPeriodService.GetLastPeriod();

            var param = CreateReportParam(message);
            var reportData = reportService.GetReportData(param);

            var grandTotal = AccumulateGrandTotal(reportData);

            return new ReportViewModel(
                message,
                BuildSportGroupSelectListItems(sportGroups, message.UserSelectedSportGroupId),
                BuildBetTypeSelectListItems(betTypes, message.UserSelectedBetTypeId),
                reportData,
                grandTotal,
                lastPeriod);
        }

        public PTHistoryViewModel Handle(PTHistoryQuery message)
            => new PTHistoryViewModel(reportService.GetPTHistories(message.CustId, message.FromDate, message.ToDate));
    }
}