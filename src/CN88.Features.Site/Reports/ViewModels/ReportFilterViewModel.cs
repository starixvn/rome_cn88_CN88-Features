namespace CN88.Features.Site.Reports.ViewModels
{
    using System.Collections.Generic;
    using CN88.Features.Reports.Models;
    using CN88.Features.Site.Shared.ViewModels;
    using Features.Shared.Models;
    using Shared.ViewModels.Components;

    public class ReportFilterViewModel : ReportFilterBaseViewModel
    {
        public ReportFilterViewModel(
            int displayMode,
            int reportType,
            bool isOwner,
            int custId,
            string custName,
            string breadcrumbLevelJson,
            UserLevel custLevel,
            SingleSelectionViewModel sportGroupSelectionViewModel,
            DateRangePickerViewModel dateRangePickerViewModel,
            SingleSelectionViewModel betTypeSelectionViewModel,
            SingleSelectionViewModel liveStatusSelectionViewModel,
            IEnumerable<SelectListItemExt> allBetTypeGroups,
            bool isMobile = false,
            bool isShowPermissionOnDownlinesMessage = false,
            AssignedDownlinesCount assignedDownlinesCount = null)
            : base(
                  displayMode,
                  reportType,
                  isOwner,
                  custId,
                  custName,
                  breadcrumbLevelJson,
                  custLevel,
                  sportGroupSelectionViewModel,
                  dateRangePickerViewModel,
                  isMobile,
                  isShowPermissionOnDownlinesMessage,
                  assignedDownlinesCount)
        {
            BetTypeSelectionViewModel = betTypeSelectionViewModel;
            LiveStatusSelectionViewModel = liveStatusSelectionViewModel;
            AllBetTypeGroups = allBetTypeGroups;
        }

        public SingleSelectionViewModel BetTypeSelectionViewModel { get; }

        public SingleSelectionViewModel LiveStatusSelectionViewModel { get; }

        public IEnumerable<SelectListItemExt> AllBetTypeGroups { get; }
    }
}