namespace CN88.Features.Site.Reports.ViewModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Constants;
    using Features.Reports.Models;
    using Features.Shared.Models;
    using Requests;
    using Shared.ViewModels.Components;
    using Shared.ViewModels;

    public abstract class IndexBaseViewModel
    {
        protected IndexBaseViewModel(
            ReportBaseQuery query,
            IEnumerable<AccountingPeriodGroup> accountingPeriodGroups,
            IEnumerable<SelectListItemExt> sportGroups)
        {
            DisplayMode = query.DisplayMode;
            CustLevelId = query.ActorLevel.IsHeadline() ? UserLevel.Super.Value : query.ActorLevel.Value;
            IsOwner = query.ActorLevel.IsNotHeadline();

            AccountingPeriodGroups = accountingPeriodGroups;
            SportGroupSelectionViewModel = new SingleSelectionViewModel(sportGroups, 0);

            ActionName = query.DisplayMode == ReportDisplayModeConstants.Wager
                ? nameof(ReportDisplayModeConstants.Wager)
                : nameof(ReportDisplayModeConstants.Legacy);
        }

        public int DisplayMode { get; }

        public int CustLevelId { get; }

        public bool IsOwner { get; }

        public string ActionName { get; }

        public IEnumerable<AccountingPeriodGroup> AccountingPeriodGroups { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; protected set; }

        public SingleSelectionViewModel SportGroupSelectionViewModel { get; }
    }
}