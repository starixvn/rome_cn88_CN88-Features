namespace CN88.Features.SubAccount.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public abstract class BaseSubAccountCommand : NonQueryCommand
    {
        protected BaseSubAccountCommand(
            int custId,
            string name,
            string pageAccess,
            string languageKey,
            string loginIP,
            int userId)
        {
            CustId = custId;
            Name = name ?? string.Empty;
            PageAccess = pageAccess;
            LanguageKey = languageKey;
            LoginIP = loginIP;
            UserId = userId;
        }

        public int CustId { get; }

        [SpParam(SqlParamName = "firstname")]
        public string Name { get; }

        public string PageAccess { get; }

        [SpParam(SqlParamName = "Language")]
        public string LanguageKey { get; }

        public string LoginIP { get; }

        public int UserId { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override bool IsValid()
            => CustId > 0
            && UserId > 0;
    }
}