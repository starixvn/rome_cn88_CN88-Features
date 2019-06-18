namespace CN88.Features.BetLists.Criteria.BetListOfReport
{
    using Params;

    public class OldSettledBetListCriteria : BetListOfReportBaseCriteria
    {
        public OldSettledBetListCriteria(BetListOfReportParam param)
            : base(param)
        {
        }

        public override string GetSettingKey() => "CS_BetList_WinLoss_Mem_Old";
    }
}