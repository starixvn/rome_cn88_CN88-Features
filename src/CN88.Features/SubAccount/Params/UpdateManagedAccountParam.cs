namespace CN88.Features.SubAccount.Params
{
    public class UpdateManagedAccountParam
    {
        public UpdateManagedAccountParam(
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
    }
}