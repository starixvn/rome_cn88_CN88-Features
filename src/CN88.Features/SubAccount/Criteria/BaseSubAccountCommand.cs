namespace CN88.Features.SubAccount.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public abstract class BaseSubAccountCommand : NonQueryCommand
    {
        public BaseSubAccountCommand(
            int custId,
            int subAccId,
            int loginSubAccId,
            string languageKey,
            string loginIP,
            int userId)
        {
            CustId = custId;
            SubAccId = subAccId;
            LoginSubAccId = loginSubAccId;
            LanguageKey = languageKey;
            LoginIP = loginIP;
            UserId = userId;
        }

        public int CustId { get; }

        public int SubAccId { get; set; }

        public int LoginSubAccId { get; }

        [SpParam(SqlParamName = "Language")]
        public string LanguageKey { get; }

        public string LoginIP { get; }

        public int UserId { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override bool IsValid()
            => CustId > 0
            && LoginSubAccId >= 0
            && UserId > 0;
    }
}
