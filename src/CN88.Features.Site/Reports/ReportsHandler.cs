namespace CN88.Features.Site.Reports
{
    using System.Threading.Tasks;
    using Features.Reports.Services;
    using Features.Shared.Services;
    using MediatR;
    using Requests;
    using ViewModels;

    public class ReportsHandler : IAsyncRequestHandler<MainQuery, MainViewModel>
    {
        private readonly SportService sportService;
        private readonly BettypeService betTypeService;
        private readonly ReportService reportService;

        public ReportsHandler(
            SportService sportService,
            BettypeService betTypeService,
            ReportService reportService)
        {
            this.sportService = sportService;
            this.betTypeService = betTypeService;
            this.reportService = reportService;
        }

        public async Task<MainViewModel> Handle(MainQuery message)
        {
            var sports = await sportService.GetSports();
            var betTypes = await betTypeService.GetBetTypesBySport();
            var accountingPeriodGroups = await reportService.GetAccountingPeriodGroups();

            return new MainViewModel(
                message.FromDate,
                message.ToDate,
                sports,
                betTypes,
                accountingPeriodGroups);
        }
    }
}