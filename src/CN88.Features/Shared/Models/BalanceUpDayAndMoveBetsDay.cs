namespace CN88.Features.Shared.Models
{
    using System;

    public class BalanceUpDayAndMoveBetsDay
    {
        public BalanceUpDayAndMoveBetsDay()
        {
        }

        public BalanceUpDayAndMoveBetsDay(DateTime balanceUpDay, DateTime moveBetsDay)
        {
            BalanceUpDay = balanceUpDay;
            MoveBetsDay = moveBetsDay;
        }

        public DateTime BalanceUpDay { get; }

        public DateTime MoveBetsDay { get; }
    }
}