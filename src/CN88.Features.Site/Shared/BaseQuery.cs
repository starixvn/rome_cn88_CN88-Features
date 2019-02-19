namespace CN88.Features.Site.Requests
{
    using Features.Models;
    using Features.Shared.Models;

    public class BaseQuery
    {
        private readonly IUser viewer;

        protected BaseQuery(IUser viewer)
        {
            this.viewer = viewer;

            // Default data, if query string had data, then get from query string.
            CustId = viewer.Id;
            CustName = viewer.Name;
            CustLevelId = viewer.LevelId;
        }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public byte CustLevelId { get; set; }

        public virtual UserLevel CustLevel => Enumeration.FromValue<UserLevel>(CustLevelId);

        public int ViewerId => viewer.Id;

        public string ViewerName => viewer.Name;

        public byte ViewerLevelId => viewer.LevelId;

        public virtual UserLevel ActorLevel => Enumeration.FromValue<UserLevel>(ViewerLevelId);

        public int SubAccountId => viewer.SubAccountId;

        public string IP => viewer.IP;

        public string Language => viewer.Language;

        public virtual bool IsValid()
            => ViewerId > 0
            && !string.IsNullOrWhiteSpace(ViewerName)
            && !string.IsNullOrWhiteSpace(Language);
    }
}