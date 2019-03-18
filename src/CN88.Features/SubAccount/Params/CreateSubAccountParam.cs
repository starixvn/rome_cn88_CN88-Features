namespace CN88.Features.SubAccount.Params
{
    using System.Collections.Generic;

    public class CreateSubAccountParam
    {
        public CreateSubAccountParam(
            int custId,
            int loginSubAccId,
            string suffix1,
            string suffix2,
            string password,
            string hashKey,
            IEnumerable<string> invalidPasswords,
            string firstName,
            string lastName,
            string pageAccess,
            string languageKey,
            string ip,
            int actorId,
            string actorName)
        {
            CustId = custId;
            LoginSubAccId = loginSubAccId;
            Suffix1 = suffix1;
            Suffix2 = suffix2;
            Password = password;
            HashKey = hashKey;
            InvalidPasswords = invalidPasswords;
            FirstName = firstName;
            LastName = lastName;
            PageAccess = pageAccess;
            LanguageKey = languageKey;
            IP = ip;
            ActorId = actorId;
            ActorName = actorName;
        }

        public int CustId { get; }

        public int LoginSubAccId { get; }

        public string Suffix1 { get; }

        public string Suffix2 { get; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string HashKey { get; }

        public IEnumerable<string> InvalidPasswords { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string PageAccess { get; set; }

        public string LanguageKey { get; }

        public string IP { get; }

        public int ActorId { get; }

        public string ActorName { get; }
    }
}
