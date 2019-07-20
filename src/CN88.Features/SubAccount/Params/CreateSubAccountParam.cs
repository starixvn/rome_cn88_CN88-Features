namespace CN88.Features.SubAccount.Params
{
    using System.Collections.Generic;

    public class CreateSubAccountParam : BaseSubAccountParam
    {
        public CreateSubAccountParam(
            string suffix1,
            string suffix2,
            string password,
            string hashKey,
            IEnumerable<string> invalidPasswords,
            string name,
            string pageAccess,
            string languageKey,
            string ip,
            int actorId,
            string actorName,
            byte roleId,
            bool isAllDownline,
            string downlineId)
            : base(name, pageAccess, languageKey, ip, actorId, roleId, isAllDownline, downlineId)
        {
            Suffix1 = suffix1;
            Suffix2 = suffix2;
            Password = password;
            HashKey = hashKey;
            InvalidPasswords = invalidPasswords;
            ActorName = actorName;
        }

        public string Suffix1 { get; }

        public string Suffix2 { get; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string HashKey { get; }

        public IEnumerable<string> InvalidPasswords { get; }

        public string ActorName { get; }
    }
}