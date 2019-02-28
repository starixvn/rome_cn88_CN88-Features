namespace CN88.Features.Site.Reports.ViewModels
{
    using System.Collections.Generic;
    using Features.Reports.Models;

    public class PTHistoryViewModel
    {
        public PTHistoryViewModel(IEnumerable<PTHistory> histories)
        {
            Histories = histories;
        }

        private IEnumerable<PTHistory> Histories { get; }
    }
}