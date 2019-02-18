namespace CN88.Features.Site.Reports
{
    using System.Threading.Tasks;
    using Features.Reports.Services;
    using Features.Shared.Services;
    using MediatR;
    using Requests;
    using Shared;
    using ViewModels;

    public class ReportsHandler : BaseHandler, IAsyncRequestHandler<MainQuery, MainViewModel>
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

        public async Task<MainViewModel> Handle(MainQuery message)
        {
            var sportGroups = await sportService.GetSportGroups();
            var betTypes = await betTypeService.GetBetTypesBySport();
            var accountingPeriodGroups = await reportService.GetAccountingPeriodGroups();
            var currentPeriod = await reportService.GetCurrentPeriod();

            return new MainViewModel(
                message.FromDate,
                message.ToDate,
                BuildSportGroupSelectListItems(sportGroups, 0),
                BuildBetTypeSelectListItems(betTypes, 0),
                accountingPeriodGroups,
                currentPeriod);
        }
    }
}