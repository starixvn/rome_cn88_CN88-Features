namespace CN88.Features.SubAccount.Params
{
    using System.Collections.Generic;
    using Criteria;

    public class CreateSubAccountParam
    {
        public CreateSubAccountParam(
            int custId,
            int loginSubAccId,
            string userName,
            string password,
            string hashKey,
            IEnumerable<string> invalidPasswords,
            string firstName,
            string lastName,
            string pageAccess,
            string languageKey,
            string ip,
            int actorId)
        {
            CustId = custId;
            LoginSubAccId = loginSubAccId;
            UserName = userName;
            Password = password;
            HashKey = hashKey;
            InvalidPasswords = invalidPasswords;
            FirstName = firstName;
            LastName = lastName;
            PageAccess = pageAccess;
            LanguageKey = languageKey;
            IP = ip;
            ActorId = actorId;
        }

        public int CustId { get; }

        public int LoginSubAccId { get; }

        public string UserName { get; }

        public string Password { get; set; }

        public string HashKey { get; }

        public IEnumerable<string> InvalidPasswords { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string PageAccess { get; set; }

        public string LanguageKey { get; }

        public string IP { get; }

        public int ActorId { get; }

        public CreateSubAccountCommand ToCommand()
            => new CreateSubAccountCommand(
                CustId,
                LoginSubAccId,
                UserName,
                Password,
                FirstName?.Trim() ?? string.Empty,
                LastName?.Trim() ?? string.Empty,
                PageAccess,
                LanguageKey,
                IP,
                ActorId);
    }
}
