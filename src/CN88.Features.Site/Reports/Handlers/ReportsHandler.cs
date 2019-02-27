namespace CN88.Features.Site.Reports.Handlers
{
    using Features.Reports.Services;
    using Features.Shared.Services;
    using MediatR;
    using Requests;
    using ViewModels;

    public class ReportsHandler : ReportsBaseHandler,
        IRequestHandler<IndexQuery, IndexViewModel>,
        IRequestHandler<ReportQuery, ReportViewModel>
    {
        private readonly SportGroupService sportService;
        private readonly BettypeGroupService betTypeService;
        private readonly ReportService reportService;

        public ReportsHandler(
            SportGroupService sportService,
            BettypeGroupService betTypeService,
            ReportService reportService)
        {
            this.sportService = sportService;
            this.betTypeService = betTypeService;
            this.reportService = reportService;
        }

        public IndexViewModel Handle(IndexQuery message)
        {
            var sportGroups = sportService.GetSportGroups();
            var betTypes = betTypeService.GetBetTypeGroupsBySport();
            var accountingPeriodGroups = reportService.GetAccountingPeriodGroups();
            var currentPeriod = reportService.GetCurrentPeriod();
            var settlementSummary = reportService.GetSettlementSummaryReport();

            return new IndexViewModel(
                message,
                BuildSportGroupSelectListItems(sportGroups, 0),
                BuildBetTypeSelectListItems(betTypes, 0),
                accountingPeriodGroups,
                currentPeriod,
                settlementSummary);
        }

        public ReportViewModel Handle(ReportQuery message)
        {
            var sportGroups = sportService.GetSportGroups();
            var betTypes = betTypeService.GetBetTypeGroupsBySport(message.UserSelectedSportGroupId);
            var currentPeriod = reportService.GetCurrentPeriod();

            var param = CreateReportParam(message);
            var reportData = reportService.GetReportData(param);

            var grandTotal = AccumulateGrandTotal(reportData);

            return new ReportViewModel(
                message,
                BuildSportGroupSelectListItems(sportGroups, message.UserSelectedSportGroupId),
                BuildBetTypeSelectListItems(betTypes, message.UserSelectedBetTypeId),
                reportData,
                grandTotal,
                currentPeriod);
        }
    }
}