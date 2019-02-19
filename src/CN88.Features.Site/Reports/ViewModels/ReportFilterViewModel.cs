namespace CN88.Features.Site.Reports.ViewModels
{
    using Features.Shared.Models;
    using Shared.ViewModels.Components;

    public class ReportFilterViewModel
    {
        public ReportFilterViewModel(
            int reportType,
            int custId,
            string custName,
            UserLevel custLevel,
            SportGroupSelectionViewModel sportGroupSelectionViewModel,
            DateRangePickerViewModel dateRangePickerViewModel,
            BetTypeSelectionViewModel betTypeSelectionViewModel)
        {
            ReportType = reportType;
            CustId = custId;
            CustName = custName;
            CustLevel = custLevel;
            SportGroupSelectionViewModel = sportGroupSelectionViewModel;
            DateRangePickerViewModel = dateRangePickerViewModel;
            BetTypeSelectionViewModel = betTypeSelectionViewModel;
        }

        public int ReportType { get; }

        public int CustId { get; }

        public string CustName { get; }

        public UserLevel CustLevel { get; }

        public SportGroupSelectionViewModel SportGroupSelectionViewModel { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public BetTypeSelectionViewModel BetTypeSelectionViewModel { get; }
    }
}