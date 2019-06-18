namespace CN88.Features.BetLists.Criteria.BetListOfReport
{
    using System;
    using Params;

    public class CancelledBetListCriteria : BetListOfReportBaseCriteria
    {
        public CancelledBetListCriteria(BetListOfReportParam param, DateTime moveBetsDate)
            : base(param)
        {
            MoveBetsDay = moveBetsDate;
        }

        public DateTime MoveBetsDay { get; }

        public override string GetSettingKey() => "CS_BetList_CancelledBet_Mem";
    }
}