namespace CN88.Features.Site.Reports.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Features.Reports.AppResources;
    using Features.Reports.Constants;
    using Features.Reports.Models;
    using Features.Shared.AppResources;
    using Features.Shared.Constants;
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
            IsNotOwner = !query.IsOwner;
            ReportLevel = query.CustLevel;
            ReportData = reportData;
            GrandTotal = grandTotal;

            var sportGroupSelectionViewModel = new SingleSelectionViewModel(sportGroups, query.UserSelectedSportGroupId);
            var dateRangePickerViewModel = new DateRangePickerViewModel(currentPeriod.FromDate, currentPeriod.ToDate, query.ToDate, query.FromDate);
            var betTypeSelectionViewModel = new SingleSelectionViewModel(betTypes, query.UserSelectedBetTypeId);

            var liveStatuses = LiveStatusConstants.LiveStatuses
                .Select(s => new SelectListItem
                {
                    Text = SharedResources.ResourceManager.GetString(s.Value),
                    Value = s.Key.ToString(),
                    Selected = s.Key == query.UserSelectedLiveStatusId
                });
            var liveStatusSelectionViewModel = new SingleSelectionViewModel(liveStatuses, query.UserSelectedLiveStatusId);

            ReportFilterViewModel = new ReportFilterViewModel(
                query.ReportType,
                query.IsOwner,
                query.CustId,
                query.CustName,
                query.CustLevel,
                sportGroupSelectionViewModel,
                dateRangePickerViewModel,
                betTypeSelectionViewModel,
                liveStatusSelectionViewModel);

            Title = query.ReportType == ReportTypeConstants.SettledWager ? ReportResources.SettledWager
                : query.ReportType == ReportTypeConstants.UnsettledWager ? ReportResources.UnsettledWager
                : query.ReportType == ReportTypeConstants.CancelledReport ? ReportResources.CancelledReport
                : ReportResources.AbnormalBets;
        }

        public bool IsOwner { get; }

        public bool IsNotOwner { get; }

        public UserLevel ReportLevel { get; }

        public IEnumerable<WinLossModel> ReportData { get; }

        public WinLossModel GrandTotal { get; }

        public ReportFilterViewModel ReportFilterViewModel { get; }

        public string Title { get; }
    }
}