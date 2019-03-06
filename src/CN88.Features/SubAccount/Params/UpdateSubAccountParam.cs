namespace CN88.Features.SubAccount.Params
{
    using Criteria;

    public class UpdateSubAccountParam
    {
        public UpdateSubAccountParam(
            int custId,
            int subAccId,
            int loginSubAccId,
            string firstName,
            string lastName,
            bool closed,
            string languageKey,
            string ip,
            int actorId)
        {
            CustId = custId;
            SubAccId = subAccId;
            LoginSubAccId = loginSubAccId;
            FirstName = firstName;
            LastName = lastName;
            Closed = closed;
            LanguageKey = languageKey;
            IP = ip;
            ActorId = actorId;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public int LoginSubAccId { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public bool Closed { get; }

        public string LanguageKey { get; }

        public string IP { get; }

        public int ActorId { get; }

        public BaseSubAccountCommand ToCommand()
            => new UpdateSubAccountCommand(
                CustId,
                SubAccId,
                LoginSubAccId,
                FirstName,
                LastName,
                Closed,
                LanguageKey,
                IP,
                ActorId);
    }
}
