namespace CN88.Features.Site.Requests
{
    using System.Collections.Generic;
    using System.Linq;
    using Features.Models;
    using Features.Shared.Models;

    public class BaseQuery
    {
        private const int HeadlineDevisor = 100;

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
            ViewerStatusId = viewer.StatusId;
            AccountManagementEditView = viewer.AccountManagementEditView;
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

        public byte ViewerStatusId { get; }

        public virtual UserLevel ActorLevel => Enumeration.FromValue<UserLevel>(ViewerLevelId);

        public IEnumerable<CustInfo> BreadcrumbLevel { get; set; } = Enumerable.Empty<CustInfo>();

        public int SubAccountId { get; }

        public bool AccountManagementEditView { get; }

        public bool IsInternal { get; }

        public string IP { get; }

        public string Language { get; }

        public string LanguageKey { get; }

        public int Divisor => ActorLevel.IsHeadline() ? HeadlineDevisor : 1;

        public virtual bool IsValid()
            => ViewerId > 0
            && !string.IsNullOrWhiteSpace(ViewerName)
            && !string.IsNullOrWhiteSpace(Language);

        public virtual string ErrorMessage { get; set; }
    }
}