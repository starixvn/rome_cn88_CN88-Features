namespace CN88.Features.Site.Reports.Handlers
{
    using CN88.Features.Shared.Constants;
    using CN88.Web.Helpers;
    using Features.Reports.Services;
    using Features.Shared.Services;
    using MediatR;
    using Requests;
    using ViewModels;

    public class OldReportsHandler :
        ReportsBaseHandler,
        IRequestHandler<OldIndexQuery, OldIndexViewModel>,
        IRequestHandler<OldReportQuery, OldReportViewModel>
    {
        private readonly AccountingPeriodService accountingPeriodService;
        private readonly ReportService reportService;
        private readonly BrowserHelper browserHelper;

        public OldReportsHandler(
            AccountingPeriodService accountingPeriodService,
            SportGroupService sportGroupService,
            ReportService reportService,
            BrowserHelper browserHelper) : base(sportGroupService)
        {
            this.accountingPeriodService = accountingPeriodService;
            this.reportService = reportService;
            this.browserHelper = browserHelper;
        }

        public OldReportViewModel Handle(OldReportQuery message)
        {
            var oldAccountingPeriods = message.IsHeadline ?
                accountingPeriodService.GetOldAccountingPeriodsForHeadline() :
                accountingPeriodService.GetOldAccountingPeriodsForAgency();

            var isShowPermissionOnDownlinesMessage = IsShowPermissionOnDownlinesMessage(message);
            var assignedDownlinesCount = isShowPermissionOnDownlinesMessage ? reportService.GetAssignedDownlinesCount(message.SubAccountId) : null;
            var param = CreateReportParam(message);
            var reportData = reportService.GetReportData(param);
            var grandTotal = AccumulateGrandTotal(reportData);

            return new OldReportViewModel(message,
                BuildSportGroupTypeSelectListItems(message.UserSelectedSportGroupId),
                reportData,
                grandTotal,
                oldAccountingPeriods,
                browserHelper.IsMobile(),
                isShowPermissionOnDownlinesMessage,
                assignedDownlinesCount);
        }

        public OldIndexViewModel Handle(OldIndexQuery message)
        {
            var accountingPeriods = accountingPeriodService.GetAccountingPeriods();
            var oldAccountingPeriods = message.IsHeadline ?
                accountingPeriodService.GetOldAccountingPeriodsForHeadline() :
                accountingPeriodService.GetOldAccountingPeriodsForAgency();

            return new OldIndexViewModel(
                message,
                BuildAccountingPeriodGroup(accountingPeriods),
                oldAccountingPeriods,
                BuildSportGroupTypeSelectListItems(SportGroupTypeConstants.AllSports));
        }
    }
}
