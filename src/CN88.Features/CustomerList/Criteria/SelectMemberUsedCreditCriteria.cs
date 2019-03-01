namespace CN88.Features.CustomerList.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class SelectMemberUsedCreditCriteria : CriteriaBase
    {
        public SelectMemberUsedCreditCriteria(int searchAccountId)
        {
            SearchAccountId = searchAccountId;
        }

        [SpParam(SqlParamName = "CustId")]
        public int SearchAccountId { get; }

        public override string GetSettingKey() => "CS_MemberUsedCredit_Sel";

        public override bool IsValid() => SearchAccountId > 0;
    }
}
