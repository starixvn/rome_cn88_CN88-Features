namespace CN88.Features.SubAccount.Criteria
{
    using Fanex.Data.Repository;

    public class GetSubAccountCriteria : CriteriaBase
    {
        public GetSubAccountCriteria(int custId, int subAccId)
        {
            CustId = custId;
            SubAccId = subAccId;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public override string GetSettingKey()
            => "Age_CustSubSelInfo";

        public override bool IsValid()
            => CustId > 0 && SubAccId > 0;
    }
}
