namespace CN88.Features.Reports.Models
{
    using System;

    public class AccountingPeriod
    {
        public int PeriodId { get; }

        public int Year { get; }

        public DateTime FromDate { get; }

        public string FromDateStr => FromDate.ToString("yyyy/MM/dd");

        public DateTime ToDate { get; }

        public string ToDateStr => ToDate.ToString("yyyy/MM/dd");
    }
}