namespace CN88.Features.Site.Reports
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Features.Reports.Models;
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

            var reportData = await reportService.GetWinLoss(
                new ReportParam(
                    message.CustId,
                    false,
                    message.FromDate,
                    message.ToDate,
                    message.SportGroupId,
                    message.BetTypeId, message.ReportType,
                    2,
                    message.CustLevel));

            var grandTotal = AccumulateGrandTotal(reportData);

            return new ReportViewModel(
                message,
                BuildSportGroupSelectListItems(sportGroups, message.SportGroupId),
                BuildBetTypeSelectListItems(betTypes, message.BetTypeId),
                reportData,
                grandTotal,
                currentPeriod);
        }

        internal WinLossModel AccumulateGrandTotal(IEnumerable<WinLossModel> reportData)
        {
            var grandTotal = new WinLossModel();

            if (reportData != null && reportData.Any())
            {
                foreach (var data in reportData)
                {
                    grandTotal.Add(data);
                }
            }

            return grandTotal;
        }
    }
}