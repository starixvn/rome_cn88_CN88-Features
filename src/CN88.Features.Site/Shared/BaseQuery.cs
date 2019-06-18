namespace CN88.Features.Site.Requests
{
    using System.Collections.Generic;
    using System.Linq;
    using Features.Models;
    using Features.Shared.Models;

    public class BaseQuery
    {
        protected BaseQuery(IUser viewer)
        {
            ViewerId = viewer.Id;
            ViewerName = viewer.Name;
            NickName = viewer.NickName;
            ViewerLevelId = viewer.LevelId;
            SubAccountId = viewer.SubAccountId;
            IsInternal = viewer.IsInternal;
            IP = viewer.IP;
            LanguageKey = viewer.LanguageKey;
            Language = viewer.Language;
        }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public byte CustLevelId { get; set; }

        public bool IsOwner { get; set; }

        public virtual UserLevel CustLevel => Enumeration.FromValue<UserLevel>(CustLevelId);

        public int ViewerId { get; }

        public string ViewerName { get; }

        public string NickName { get; }

        public byte ViewerLevelId { get; }

        public virtual UserLevel ActorLevel => Enumeration.FromValue<UserLevel>(ViewerLevelId);

        public IEnumerable<CustInfo> BreadcrumbLevel { get; set; } = Enumerable.Empty<CustInfo>();

        public int SubAccountId { get; }

        public bool IsInternal { get; }

        public string IP { get; }

        public string LanguageKey { get; }

        public string Language { get; }

        public virtual bool IsValid()
            => ViewerId > 0
            && !string.IsNullOrWhiteSpace(ViewerName)
            && !string.IsNullOrWhiteSpace(Language);
    }
}