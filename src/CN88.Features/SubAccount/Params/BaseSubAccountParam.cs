namespace CN88.Features.SubAccount.Params
{
    public class BaseSubAccountParam
    {
        protected BaseSubAccountParam(
            string name,
            string pageAccess,
            string languageKey,
            string ip,
            int actorId,
            byte roleId,
            bool isAllDownline,
            string downlineId)
        {
            Name = name;
            PageAccess = pageAccess;
            LanguageKey = languageKey;
            IP = ip;
            ActorId = actorId;
            RoleId = roleId;
            IsAllDownline = isAllDownline;
            DownlineId = downlineId;
        }

        public string Name { get; }

        public string PageAccess { get; set; }

        public string LanguageKey { get; }

        public string IP { get; }

        public int ActorId { get; }

        public byte RoleId { get; }

        public bool IsAllDownline { get; }

        public string DownlineId { get; }
    }
}