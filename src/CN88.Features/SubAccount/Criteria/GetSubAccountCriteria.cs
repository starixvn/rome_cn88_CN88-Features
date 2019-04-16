namespace CN88.Features.SubAccount.Criteria
{
    public class GetSubAccountCriteria : GetSubAccountListCriteria
    {
        public GetSubAccountCriteria(int custId, int subAccId)
            : base(custId)
        {
            SubAccId = subAccId;
        }

        public int SubAccId { get; }

        public override string GetSettingKey()
            => "CS_CustSubSelInfo";

        public override bool IsValid()
            => base.IsValid() && SubAccId > 0;
    }
}
