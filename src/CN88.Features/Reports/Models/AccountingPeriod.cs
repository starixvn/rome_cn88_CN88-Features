namespace CN88.Features.Reports.Models
{
    using System;
    using Shared.Utils;

    public class AccountingPeriod
    {
        public AccountingPeriod()
        {
        }

        public AccountingPeriod(
            int periodId,
            int year,
            DateTime fromDate,
            DateTime toDate)
        {
            PeriodId = periodId;
            Year = year;
            FromDate = fromDate;
            ToDate = toDate;
        }

        public int PeriodId { get; }

        public int Year { get; }

        public DateTime FromDate { get; }

        public string FromDateStr => FromDate == DateTime.MinValue ? string.Empty : FromDate.ToString("yyyy/MM/dd");

        public DateTime ToDate { get; }

        public string ToDateStr => ToDate == DateTime.MinValue ? string.Empty : ToDate.ToString("yyyy/MM/dd");

        public bool IsCurrentPeriod => FromDate <= SystemTime.Today && SystemTime.Today <= ToDate;
    }
}