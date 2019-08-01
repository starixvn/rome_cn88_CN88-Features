namespace CN88.Features.SubAccount.Criteria
{
    using Fanex.Data.Repository;

    public class GetManagedAccountCriteria : CriteriaBase
    {
        public GetManagedAccountCriteria(int custId, byte roleId, int subAccId)
        {
            CustId = custId;
            RoleId = roleId;
            SubAccId = subAccId;
        }

        public int CustId { get; }

        public byte RoleId { get; }

        public int SubAccId { get; }

        public bool IsAllDownline { get; set; }

        public override string GetSettingKey() => "CS_SubAccManagementSel";

        public override bool IsValid() => CustId > 0;
    }
}