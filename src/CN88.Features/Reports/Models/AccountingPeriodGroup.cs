namespace CN88.Features.Reports.Models
{
    using System.Collections.Generic;

    public class AccountingPeriodGroup
    {
        public AccountingPeriodGroup(string name, List<AccountingPeriod> periods, int year)
        {
            Name = name;
            Periods = periods;
            Year = year;
        }

        public string Name { get; set; }

        public List<AccountingPeriod> Periods { get; set; }

        public int Year { get; set; }
    }
}