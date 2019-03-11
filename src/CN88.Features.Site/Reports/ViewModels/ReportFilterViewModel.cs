namespace CN88.Features.Site.Reports.ViewModels
{
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
            SingleSelectionViewModel allBetTypesSelectionViewModel = null)
            : base(displayMode, reportType, isOwner, custId, custName, breadcrumbLevelJson, custLevel, sportGroupSelectionViewModel, dateRangePickerViewModel)
        {
            BetTypeSelectionViewModel = betTypeSelectionViewModel;
            LiveStatusSelectionViewModel = liveStatusSelectionViewModel;
            AllBetTypesSelectionViewModel = allBetTypesSelectionViewModel;
        }

        public SingleSelectionViewModel BetTypeSelectionViewModel { get; }

        public SingleSelectionViewModel LiveStatusSelectionViewModel { get; }

        public SingleSelectionViewModel AllBetTypesSelectionViewModel { get; }
    }
}