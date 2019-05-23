namespace CN88.Features.Site.Reports.Handlers
{
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
        private readonly SportGroupService sportGroupService;
        private readonly ReportService reportService;
        private readonly BrowserHelper browserHelper;

        public OldReportsHandler(
            AccountingPeriodService accountingPeriodService,
            SportGroupService sportGroupService,
            ReportService reportService,
            BrowserHelper browserHelper)
        {
            this.accountingPeriodService = accountingPeriodService;
            this.sportGroupService = sportGroupService;
            this.reportService = reportService;
            this.browserHelper = browserHelper;
        }

        public OldReportViewModel Handle(OldReportQuery message)
        {
            var oldAccountingPeriods = accountingPeriodService.GetOldAccountingPeriods();
            var sportGroups = sportGroupService.GetSportGroups();

            var param = CreateReportParam(message);
            var reportData = reportService.GetReportData(param);

            var grandTotal = AccumulateGrandTotal(reportData);

            return new OldReportViewModel(message,
                BuildSportGroupSelectListItems(sportGroups, message.UserSelectedSportGroupId),
                reportData,
                grandTotal,
                oldAccountingPeriods,
                browserHelper.IsMobile());
        }

        public OldIndexViewModel Handle(OldIndexQuery message)
        {
            var accountingPeriods = accountingPeriodService.GetAccountingPeriods();
            var oldAccountingPeriods = accountingPeriodService.GetOldAccountingPeriods();
            var sportGroups = sportGroupService.GetSportGroups();

            return new OldIndexViewModel(
                message,
                BuildAccountingPeriodGroup(accountingPeriods),
                oldAccountingPeriods,
                BuildSportGroupSelectListItems(sportGroups, 0));
        }
    }
}