namespace CN88.Features.BetLists.Criteria.BetListOfReport
{
    using System;
    using Params;

    public class AbnormalBetListCriteria : BetListOfReportBaseCriteria
    {
        public AbnormalBetListCriteria(BetListOfReportParam param, DateTime moveBetsDate)
            : base(param)
        {
            MoveBetsDay = moveBetsDate;
        }

        public DateTime MoveBetsDay { get; }

        public override string GetSettingKey() => "CS_BetList_AbnormalBet_Mem";
    }
}