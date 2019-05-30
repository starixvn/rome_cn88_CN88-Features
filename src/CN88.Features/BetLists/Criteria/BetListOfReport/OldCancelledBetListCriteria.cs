namespace CN88.Features.BetLists.Criteria.BetListOfReport
{
    using Params;

    public class OldCancelledBetListCriteria : BetListOfReportBaseCriteria
    {
        public OldCancelledBetListCriteria(BetListOfReportParam param)
            : base(param)
        {
        }

        public override string GetSettingKey() => "CS_BetList_CancelledBet_Mem_Old";
    }
}