namespace CN88.Features.Site.Reports
{
    using System.Collections.Generic;
    using System.Linq;
    using Features.Reports.Models;
    using Features.Reports.Params;
    using Features.Reports.Services;
    using Features.Shared.Services;
    using MediatR;
    using Requests;
    using Shared;
    using ViewModels;

    public class ReportsHandler : BaseHandler,
        IRequestHandler<IndexQuery, IndexViewModel>,
        IRequestHandler<ReportQuery, ReportViewModel>
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

        public IndexViewModel Handle(IndexQuery message)
        {
            var sportGroups = sportService.GetSportGroups();
            var betTypes = betTypeService.GetBetTypesBySport();
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
            var betTypes = betTypeService.GetBetTypesBySport(message.UserSelectedSportGroupId);
            var currentPeriod = reportService.GetCurrentPeriod();
            var param = new ReportParam(
                     message.CustId,
                     message.IsOwner,
                     message.FromDate,
                     message.ToDate,
                     message.UserSelectedSportGroupId,
                     message.UserSelectedBetTypeId,
                     message.ReportType,
                     message.UserSelectedLiveStatusId,
                     message.CustLevel);

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

        internal ReportModel AccumulateGrandTotal(IEnumerable<ReportModel> reportData)
        {
            var grandTotal = new ReportModel();

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