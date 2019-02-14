namespace CN88.Features.CustomerList.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class SelectCreditRangeCriteria : CriteriaBase
    {
        public SelectCreditRangeCriteria(int searchAccountId)
        {
            SearchAccountId = searchAccountId;
        }

        [SpParam(SqlParamName = "CustId")]
        public int SearchAccountId { get; }

        public override string GetSettingKey() => "Age_CS_Customer_Credit_Sel";

        public override bool IsValid() => SearchAccountId > 0;
    }
}
