namespace CN88.Features.SubAccount.Params
{
    public class BaseSubAccountParam
    {
        protected BaseSubAccountParam(
            string name,
            string pageAccess,
            string languageKey,
            string ip,
            int actorId)
        {
            Name = name;
            PageAccess = pageAccess;
            LanguageKey = languageKey;
            IP = ip;
            ActorId = actorId;
        }

        public string Name { get; }

        public string PageAccess { get; set; }

        public string LanguageKey { get; }

        public string IP { get; }

        public int ActorId { get; }
    }
}
