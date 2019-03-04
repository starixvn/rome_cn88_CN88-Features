namespace CN88.Features.Reports.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class ReCheckCustomerPasswordCommand : NonQueryCommand
    {
        public ReCheckCustomerPasswordCommand(
            int custId,
            int subAccId,
            string userPassword)
        {
            CustId = custId;
            SubAccId = subAccId;
            UserPassword = userPassword;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        [SpParam(SqlParamName = "UserPwd")]
        public string UserPassword { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "CS_CustomerReCheckPwd";

        public override bool IsValid() => CustId > 0 && !string.IsNullOrWhiteSpace(UserPassword);
    }
}