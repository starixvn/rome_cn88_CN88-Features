namespace CN88.Features.Site.Reports
{
    using System.Threading.Tasks;
    using Features.Reports.Params;
    using Features.Reports.Services;
    using Features.Shared.Services;
    using MediatR;
    using Requests;
    using Shared;
    using ViewModels;

    public class ReportsHandler : BaseHandler,
        IAsyncRequestHandler<IndexQuery, IndexViewModel>,
        IAsyncRequestHandler<ReportQuery, ReportViewModel>
    {
        private readonly SportGroupService sportService;
        private readonly BettypeService betTypeService;
        private readonly ReportService reportService;

        public ReportsHandler(
            SportGroupService sportService,
            BettypeService betTypeService,
            ReportService reportService)
        {
            this.sportService = sportService;
            this.betTypeService = betTypeService;
            this.reportService = reportService;
        }

        public async Task<IndexViewModel> Handle(IndexQuery message)
        {
            var sportGroups = await sportService.GetSportGroups();
            var betTypes = await betTypeService.GetBetTypesBySport();
            var accountingPeriodGroups = await reportService.GetAccountingPeriodGroups();
            var currentPeriod = await reportService.GetCurrentPeriod();

            return new IndexViewModel(
                message,
                BuildSportGroupSelectListItems(sportGroups, 0),
                BuildBetTypeSelectListItems(betTypes, 0),
                accountingPeriodGroups,
                currentPeriod);
        }

        public async Task<ReportViewModel> Handle(ReportQuery message)
        {
            var sportGroups = await sportService.GetSportGroups();
            var betTypes = await betTypeService.GetBetTypesBySport(message.SportGroupId);
            var currentPeriod = await reportService.GetCurrentPeriod();

            var reportData = await reportService.GetReportData(
                new ReportParam(
                    message.CustId,
                    false,
                    message.FromDate,
                    message.ToDate,
                    message.SportGroupId,
                    message.BetTypeId, message.ReportType,
                    2,
                    message.CustLevel));

            return new ReportViewModel(
                message,
                BuildSportGroupSelectListItems(sportGroups, message.SportGroupId),
                BuildBetTypeSelectListItems(betTypes, message.BetTypeId),
                reportData,
                currentPeriod);
        }
    }
}