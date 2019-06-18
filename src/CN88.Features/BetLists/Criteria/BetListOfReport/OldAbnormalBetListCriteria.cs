namespace CN88.Features.BetLists.Criteria.BetListOfReport
{
    using Params;

    public class OldAbnormalBetListCriteria : BetListOfReportBaseCriteria
    {
        public OldAbnormalBetListCriteria(BetListOfReportParam param)
            : base(param)
        {
        }

        public override string GetSettingKey() => "CS_BetList_AbnormalBet_Mem_Old";
    }
}