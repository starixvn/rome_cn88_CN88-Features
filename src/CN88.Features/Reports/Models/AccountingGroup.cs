namespace CN88.Features.Reports.Models
{
    using System.Collections.Generic;

    public class AccountingGroup
    {
        public AccountingGroup(string name, List<AccountingPeriod> periods)
        {
            Name = name;
            Periods = periods;
        }

        public string Name { get; set; }

        public List<AccountingPeriod> Periods { get; set; }
    }
}
