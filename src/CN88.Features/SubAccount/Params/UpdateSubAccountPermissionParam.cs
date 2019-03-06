namespace CN88.Features.SubAccount.Params
{
    using Criteria;

    public class UpdateSubAccountPermissionParam
    {
        public UpdateSubAccountPermissionParam(
            int custId,
            int subAccId,
            int loginSubAccId,
            string pageAccess,
            string languageKey,
            string ip,
            int actorId)
        {
            CustId = custId;
            SubAccId = subAccId;
            LoginSubAccId = loginSubAccId;
            PageAccess = pageAccess;
            LanguageKey = languageKey;
            IP = ip;
            ActorId = actorId;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public int LoginSubAccId { get; }

        public string PageAccess { get; }

        public string LanguageKey { get; }

        public string IP { get; }

        public int ActorId { get; }

        public UpdateSubAccountPermissionCommand ToCommand()
            => new UpdateSubAccountPermissionCommand(
                CustId,
                SubAccId,
                LoginSubAccId,
                PageAccess,
                LanguageKey,
                IP,
                ActorId);
    }
}
