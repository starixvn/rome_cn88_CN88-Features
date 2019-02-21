namespace CN88.Features.SubAccount.Criteria
{
    public class UpdateSubAccountPermissionCommand : BaseSubAccountCommand
    {
        public UpdateSubAccountPermissionCommand(
            int custId,
            int subAccId,
            int loginSubAccId,
            string updPageAccess,
            string languageKey,
            string loginIP,
            int userId) : base(custId, subAccId, loginSubAccId, languageKey, loginIP, userId)
        {
            UpdPageAccess = updPageAccess;
        }

        public string UpdPageAccess { get; }

        public override string GetSettingKey()
            => "Age_CustSubUpdPageAccess";

        public override bool IsValid()
            => base.IsValid()
            && SubAccId > 0
            && !string.IsNullOrWhiteSpace(UpdPageAccess);
    }
}
