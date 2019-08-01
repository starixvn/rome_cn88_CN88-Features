namespace CN88.Features.Site.Reports.Handlers
{
    using System.Linq;
    using CN88.Features.Reports.Models;
    using CN88.Web.Helpers;
    using Features.Reports.Services;
    using Features.Shared.Services;
    using MediatR;
    using Requests;
    using ViewModels;

    public class ReportsHandler : ReportsBaseHandler,
        IRequestHandler<IndexQuery, IndexViewModel>,
        IRequestHandler<ReportQuery, ReportViewModel>,
        IRequestHandler<PTHistoryQuery, PTHistoryViewModel>,
        IRequestHandler<SettlementSummaryQuery, SettlementSummaryReport>,
        IRequestHandler<AccountingSheetQuery, AccountingSheetViewModel>
    {
        private readonly SportGroupService sportGroupService;
        private readonly BettypeGroupService betTypeService;
        private readonly AccountingPeriodService accountingPeriodService;
        private readonly SettlementSummaryService settlementSummaryService;
        private readonly ReportService reportService;
        private readonly BrowserHelper browserHelper;

        public ReportsHandler(
            SportGroupService sportGroupService,
            BettypeGroupService betTypeService,
            AccountingPeriodService accountingPeriodService,
            SettlementSummaryService settlementSummaryService,
            ReportService reportService,
            BrowserHelper browserHelper)
        {
            this.sportGroupService = sportGroupService;
            this.betTypeService = betTypeService;
            this.accountingPeriodService = accountingPeriodService;
            this.settlementSummaryService = settlementSummaryService;
            this.reportService = reportService;
            this.browserHelper = browserHelper;
        }

        public IndexViewModel Handle(IndexQuery message)
        {
            var sportGroups = sportGroupService.GetSportGroups();
            var allBetTypeGroups = betTypeService.GetBetTypeGroups();
            var lastPeriod = accountingPeriodService.GetLastPeriod();
            var currentPeriod = accountingPeriodService.GetCurrentPeriod();

            return new IndexViewModel(
                message,
                browserHelper.IsMobile(),
                currentPeriod,
                lastPeriod,
                BuildSportGroupSelectListItems(sportGroups, 0),
                BuildBetTypeSelectListItems(allBetTypeGroups, 0),
                BuildBetTypeSelectListItems(
                    allBetTypeGroups.Where(t => t.SportTypeGroup == message.UserSelectedSportGroupId),
                    message.UserSelectedBetTypeId));
        }

        public ReportViewModel Handle(ReportQuery message)
        {
            var lastPeriod = accountingPeriodService.GetLastPeriod();
            message.FromDate = message.FromDate < lastPeriod.FromDate ? lastPeriod.FromDate : message.FromDate;
            var sportGroups = sportGroupService.GetSportGroups();
            var allBetTypes = betTypeService.GetBetTypeGroups();
            var betTypes = allBetTypes.Where(betType => betType.SportTypeGroup == message.UserSelectedSportGroupId);

            var isShowPermissionOnDownlinesMessage = IsShowPermissionOnDownlinesMessage(message);
            var assignedDownlinesCount = isShowPermissionOnDownlinesMessage ? reportService.GetAssignedDownlinesCount(message.SubAccountId) : null;

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
                browserHelper.IsMobile(),
                isShowPermissionOnDownlinesMessage,
                assignedDownlinesCount,
                BuildBetTypeSelectListItems(allBetTypes, 0));
        }

        public PTHistoryViewModel Handle(PTHistoryQuery message)
            => new PTHistoryViewModel(reportService.GetPTHistories(message.CustId, message.FromDate, message.ToDate));

        public SettlementSummaryReport Handle(SettlementSummaryQuery message)
            => settlementSummaryService.GetSettlementSummaryReport();

        public AccountingSheetViewModel Handle(AccountingSheetQuery message)
        {
            var currentPeriod = accountingPeriodService.GetCurrentPeriod();
            var lastPeriod = accountingPeriodService.GetLastPeriod();
            var accountingPeriods = accountingPeriodService.GetAccountingPeriods();
            var accountingPeriodGroups = BuildAccountingPeriodGroup(accountingPeriods);

            return new AccountingSheetViewModel(accountingPeriodGroups, currentPeriod, lastPeriod);
        }
    }
}
