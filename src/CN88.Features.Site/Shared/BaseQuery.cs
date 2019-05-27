namespace CN88.Features.Site.Requests
{
    using System.Collections.Generic;
    using System.Linq;
    using CN88.Features.CustomerList;
    using Features.Models;
    using Features.Shared.Models;

    public class BaseQuery
    {
        protected BaseQuery(IUser viewer)
        {
            LanguageKey = viewer.LanguageKey;
        }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public byte CustLevelId { get; set; }

        public bool IsOwner { get; set; }

        public virtual UserLevel CustLevel => Enumeration.FromValue<UserLevel>(CustLevelId);

        public int ViewerId => viewer.Id;

        public string ViewerName => viewer.Name;

        public string NickName => viewer.NickName;

        public byte ViewerLevelId => viewer.LevelId;

        public byte ViewerStatusId => viewer.StatusId;

        public bool IsActiveViewer => viewer.StatusId.Equals(CustomerStatus.Active);

        public virtual UserLevel ActorLevel => Enumeration.FromValue<UserLevel>(ViewerLevelId);

        public IEnumerable<CustInfo> BreadcrumbLevel { get; set; } = Enumerable.Empty<CustInfo>();

        public int SubAccountId => viewer.SubAccountId;

        public bool IsInternal => viewer.IsInternal;

        public string IP => viewer.IP;

        public string Language => viewer.Language;

        public string LanguageKey { get; }


        public virtual bool IsValid()
            => ViewerId > 0
            && !string.IsNullOrWhiteSpace(ViewerName)
            && !string.IsNullOrWhiteSpace(Language);
    }
}