namespace CN88.Features.Site.Reports.ViewModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Features.Reports.AppResources;
    using Features.Reports.Constants;
    using Features.Reports.Models;
    using Features.Shared.Models;
    using Requests;
    using Shared.ViewModels.Components;

    public class ReportViewModel
    {
        public ReportViewModel(
            ReportQuery query,
            IEnumerable<SelectListItem> sportGroups,
            IEnumerable<SelectListItem> betTypes,
            IEnumerable<WinLossModel> reportData,
            WinLossModel grandTotal,
            AccountingPeriod currentPeriod)
        {
            ReportLevel = query.CustLevel;
            ReportData = reportData;
            GrandTotal = grandTotal;

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

        public UserLevel ReportLevel { get; }

        public IEnumerable<WinLossModel> ReportData { get; }

        public WinLossModel GrandTotal { get; }

        public ReportFilterViewModel ReportFilterViewModel { get; }

        public string Title { get; }
    }
}