namespace CN88.Features.BetLists.Criteria.BetListOfReport
{
    using Params;

    public class UnsettledBetListCriteria : BetListOfReportBaseCriteria
    {
        public UnsettledBetListCriteria(BetListOfReportParam param)
            : base(param)
        {
        }

        public override string GetSettingKey() => "CS_BetList_Unsettled_Mem";
    }
}