namespace CN88.Features.Site.Reports.Handlers
{
    using Features.Reports.Services;
    using Features.Shared.Services;
    using MediatR;
    using Requests;
    using Shared.ViewModels;
    using System.Linq;
    using ViewModels;

    public class ReportsHandler : ReportsBaseHandler,
        IRequestHandler<IndexQuery, IndexViewModel>,
        IRequestHandler<ReportQuery, ReportViewModel>,
        IRequestHandler<PTHistoryQuery, PTHistoryViewModel>,
        IRequestHandler<BettypeQuery, BettypeViewModel>
    {
        private readonly SportGroupService sportGroupService;
        private readonly BettypeGroupService betTypeService;
        private readonly AccountingPeriodService accountingPeriodService;
        private readonly SettlementSummaryService settlementSummaryService;
        private readonly ReportService reportService;

        public ReportsHandler(
            SportGroupService sportGroupService,
            BettypeGroupService betTypeService,
            AccountingPeriodService accountingPeriodService,
            SettlementSummaryService settlementSummaryService,
            ReportService reportService)
        {
            this.sportGroupService = sportGroupService;
            this.betTypeService = betTypeService;
            this.accountingPeriodService = accountingPeriodService;
            this.settlementSummaryService = settlementSummaryService;
            this.reportService = reportService;
        }

        public IndexViewModel Handle(IndexQuery message)
        {
            var settlementSummary = settlementSummaryService.GetSettlementSummaryReport();
            var accountingPeriods = accountingPeriodService.GetAccountingPeriods();
            var currentPeriod = accountingPeriodService.GetCurrentPeriod();
            var lastPeriod = accountingPeriodService.GetLastPeriod();
            var sportGroups = sportGroupService.GetSportGroups();
            var betTypes = betTypeService.GetBetTypeGroupsBySport();

            return new IndexViewModel(
                message,
                settlementSummary,
                BuildAccountingPeriodGroup(accountingPeriods),
                currentPeriod,
                lastPeriod,
                BuildSportGroupSelectListItems(sportGroups, 0),
                BuildBetTypeSelectListItems(betTypes, 0));
        }

        public ReportViewModel Handle(ReportQuery message)
        {
            var sportGroups = sportGroupService.GetSportGroups();
            var allBetTypes = betTypeService.GetBetTypeGroupsBySport(0);
            var betTypes = allBetTypes.Where(betType => betType.SportTypeGroup == message.UserSelectedSportGroupId);
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
                lastPeriod,
                BuildBetTypeSelectListItems(allBetTypes, 0));
        }

        public PTHistoryViewModel Handle(PTHistoryQuery message)
            => new PTHistoryViewModel(reportService.GetPTHistories(message.CustId, message.FromDate, message.ToDate));

        public BettypeViewModel Handle(BettypeQuery message)
        {
            var betTypes = betTypeService.GetBetTypeGroupsBySport(message.SportType);

            return new BettypeViewModel(BuildBetTypeSelectListItems(betTypes, 0));
        }
    }
}