namespace CN88.Features.Site.Reports.ViewModels
{
    public class AccountingPeriodViewModel
    {
        public AccountingPeriodViewModel(
            int index,
            string currentYearPeriod,
            string lastYearPeriod)
        {
            Index = index;
            CurrentYearPeriod = currentYearPeriod;
            LastYearPeriod = lastYearPeriod;
        }

        public int Index { get; }

        public string CurrentYearPeriod { get; }

        public string LastYearPeriod { get; }
    }
}