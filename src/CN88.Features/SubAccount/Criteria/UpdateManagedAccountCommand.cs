namespace CN88.Features.SubAccount.Criteria
{
    using Fanex.Data.Repository;

    public class UpdateManagedAccountCommand : NonQueryCommand
    {
        public UpdateManagedAccountCommand(
            int custId,
            byte roleId,
            int subAccId,
            bool isAllDownline,
            string downlineId,
            string language,
            string loginIP,
            int userId)
        {
            CustId = custId;
            RoleId = roleId;
            SubAccId = subAccId;
            IsAllDownline = isAllDownline;
            DownlineId = downlineId;
            Language = language;
            LoginIP = loginIP;
            UserId = userId;
        }

        public int CustId { get; }

        public byte RoleId { get; }

        public int SubAccId { get; }

        public bool IsAllDownline { get; }

        public string DownlineId { get; }

        public string Language { get; }

        public string LoginIP { get; }

        public int UserId { get; }

        public bool IsUpdated { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "CS_SubAccManagementUpd";

        public override bool IsValid() => CustId > 0 && RoleId > 0 && SubAccId > 0;
    }
}