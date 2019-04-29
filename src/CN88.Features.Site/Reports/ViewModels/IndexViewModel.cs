namespace CN88.Features.Site.Reports.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Features.Reports.AppResources;
    using Features.Reports.Constants;
    using Features.Reports.Models;
    using Features.Shared.AppResources;
    using Features.Shared.Constants;
    using Features.Shared.Utils;
    using Requests;
    using Shared.ViewModels;
    using Shared.ViewModels.Components;

    public class IndexViewModel : IndexBaseViewModel
    {
        public IndexViewModel(
            IndexQuery query,
            AccountingPeriod currentPeriod,
            AccountingPeriod lastPeriod,
            IEnumerable<SelectListItemExt> sportGroups,
            IEnumerable<SelectListItemExt> betTypes)
            : base(query, sportGroups)
        {
            DateRangePickerViewModel = new DateRangePickerViewModel(
                lastPeriod.FromDate,
                DateTime.MaxValue,
                SystemTime.Today.AddDays(-1),
                SystemTime.Today.AddDays(-1));
            BetTypeSelectionViewModel = new SingleSelectionViewModel(betTypes, 0);
            CurrentPeriod = currentPeriod;
            LastPeriod = lastPeriod;

            var liveStatuses = LiveStatusConstants.LiveStatuses
                .Select(s => new SelectListItemExt
                {
                    Text = SharedResources.ResourceManager.GetString(s.Value),
                    Value = s.Key.ToString(),
                });
            LiveStatusSelectionViewModel = new SingleSelectionViewModel(liveStatuses, LiveStatusConstants.AllMarkets);
        }

        public SingleSelectionViewModel BetTypeSelectionViewModel { get; }

        public SingleSelectionViewModel LiveStatusSelectionViewModel { get; }

        public AccountingPeriod CurrentPeriod { get; }

        public AccountingPeriod LastPeriod { get; }

        public IEnumerable<SelectListItem> WagerReportTypes => ReportTypeConstants.WagerReportTypes
            .Select(t => new SelectListItem
            {
                Text = ReportResources.ResourceManager.GetString(t.Value),
                Value = t.Key.ToString(),
                Selected = t.Key == ReportTypeConstants.SettledWager
            });

        public IEnumerable<SelectListItem> CancelledReportTypes => ReportTypeConstants.CancelledReportTypes
            .Select(t => new SelectListItem
            {
                Text = ReportResources.ResourceManager.GetString(t.Value),
                Value = t.Key.ToString(),
                Selected = t.Key == ReportTypeConstants.CancelledReport
            });
    }
}