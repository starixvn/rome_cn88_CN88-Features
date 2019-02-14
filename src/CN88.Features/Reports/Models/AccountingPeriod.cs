using System;

namespace CN88.Features.Reports.Models
{
    public class AccountingPeriod
    {
        public int PeriodId { get; }

        public int Year { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }
    }
}