namespace CN88.Features.SubAccount.Criteria
{
    using Fanex.Data.Repository;

    public class GetSubAccountListCriteria : CriteriaBase
    {
        public GetSubAccountListCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public override string GetSettingKey()
            => "Age_CustSubSel";

        public override bool IsValid()
            => CustId > 0;
    }
}
