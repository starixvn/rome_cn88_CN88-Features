namespace CN88.Features.Reports.Criteria
{
    using Fanex.Data.Repository;

    public class GetSubAccountAssignedDownlinesCount : CriteriaBase
    {
        public GetSubAccountAssignedDownlinesCount(int subAccId)
        {
            SubAccId = subAccId;
        }

        public int SubAccId { get; }

        public override string GetSettingKey() => "CS_SubAcc_AssignedDownlinesCount";

        public override bool IsValid() => SubAccId > 0;
    }
}