namespace CN88.Features.Reports.Models
{
    using System.Collections.Generic;

    public class SettlementSummaryReport
    {
        public SettlementSummaryReport(
            IList<string> sportGroups,
            SettlementByGroup yesterday,
            SettlementByGroup today)
        {
            SportGroups = sportGroups;
            Yesterday = yesterday;
            Today = today;
        }

        public IList<string> SportGroups { get; }

        public SettlementByGroup Yesterday { get; }

        public SettlementByGroup Today { get; }
    }
}