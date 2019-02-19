namespace CN88.Features.Site.Reports.ViewModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Features.Reports.AppResources;
    using Features.Reports.Constants;
    using Features.Reports.Models;
    using Requests;
    using Shared.ViewModels.Components;

    public class ReportViewModel
    {
        public ReportViewModel(
            ReportQuery query,
            IEnumerable<SelectListItem> sportGroups,
            IEnumerable<SelectListItem> betTypes,
            IEnumerable<ReportModel> reportData,
            AccountingPeriod currentPeriod)
        {
            ReportData = reportData;
            var sportGroupSelectionViewModel = new SportGroupSelectionViewModel(sportGroups, query.SportGroupId);
            var dateRangePickerViewModel = new DateRangePickerViewModel(currentPeriod.FromDate, currentPeriod.ToDate, query.ToDate, query.FromDate);
            var betTypeSelectionViewModel = new BetTypeSelectionViewModel(betTypes, query.BetTypeId);

            ReportFilterViewModel = new ReportFilterViewModel(
                query.ReportType,
                query.CustId,
                query.CustName,
                query.CustLevel,
                sportGroupSelectionViewModel,
                dateRangePickerViewModel,
                betTypeSelectionViewModel);

            Title = query.ReportType == ReportTypeConstants.SettledWager ? ReportResources.SettledWager
                : query.ReportType == ReportTypeConstants.UnsettledWager ? ReportResources.UnsettledWager
                : query.ReportType == ReportTypeConstants.CancelledReport ? ReportResources.CancelledReport
                : ReportResources.AbnormalBets;
        }

        public IEnumerable<ReportModel> ReportData { get; }

        public ReportFilterViewModel ReportFilterViewModel { get; }

        public string Title { get; }
    }
}