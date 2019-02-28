namespace CN88.Features.Reports.Criteria
{
    using Fanex.Data.Repository;

    public class GetCustomerPTSettingCriteria : CriteriaBase
    {
        public GetCustomerPTSettingCriteria(
            int roleId,
            string custIds,
            bool isTest)
        {
            RoleId = roleId;
            CustIds = custIds;
            IsTest = isTest;
        }

        public int RoleId { get; }

        public string CustIds { get; }

        public bool IsTest { get; }

        public override string GetSettingKey() => "CS_CustSetting_LegacyReportSetting_Sel";

        public override bool IsValid() => RoleId > 0 && !string.IsNullOrWhiteSpace(CustIds);
    }
}