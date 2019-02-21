namespace CN88.Features.Site.Reports.ViewModels
{
    using Features.Shared.Models;
    using Shared.ViewModels.Components;

    public class ReportFilterViewModel
    {
        public ReportFilterViewModel(
            int reportType,
            bool isOwner,
            int custId,
            string custName,
            UserLevel custLevel,
            SingleSelectionViewModel sportGroupSelectionViewModel,
            DateRangePickerViewModel dateRangePickerViewModel,
            SingleSelectionViewModel betTypeSelectionViewModel,
            SingleSelectionViewModel liveStatusSelectionViewModel)
        {
            ReportType = reportType;
            IsOwner = isOwner;
            CustId = custId;
            CustName = custName;
            CustLevel = custLevel;
            SportGroupSelectionViewModel = sportGroupSelectionViewModel;
            DateRangePickerViewModel = dateRangePickerViewModel;
            BetTypeSelectionViewModel = betTypeSelectionViewModel;
            LiveStatusSelectionViewModel = liveStatusSelectionViewModel;
        }

        public int ReportType { get; }

        public bool IsOwner { get; }

        public int CustId { get; }

        public string CustName { get; }

        public UserLevel CustLevel { get; }

        public SingleSelectionViewModel SportGroupSelectionViewModel { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public SingleSelectionViewModel BetTypeSelectionViewModel { get; }

        public SingleSelectionViewModel LiveStatusSelectionViewModel { get; }
    }
}