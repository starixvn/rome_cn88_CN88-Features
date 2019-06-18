namespace CN88.Features.BetLists.Criteria.BetListOfReport
{
    using System;
    using Params;

    public class SettledBetListCriteria : BetListOfReportBaseCriteria
    {
        public SettledBetListCriteria(BetListOfReportParam param, DateTime moveBetsDate)
            : base(param)
        {
            MoveBetsDay = moveBetsDate;
        }

        public DateTime MoveBetsDay { get; }

        public override string GetSettingKey() => "CS_BetList_WinLoss_Mem";
    }
}