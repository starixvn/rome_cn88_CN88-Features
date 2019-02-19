namespace CN88.Features.Site.Requests
{
    using CN88.Features.Models;
    using CN88.Features.Shared.Models;

    public class BaseQuery
    {
        private readonly IUser viewer;

        protected BaseQuery(IUser viewer)
        {
            //this.viewer = viewer;

            //// Default data, if query string had data, then get from query string.
            //CustId = viewer.Id;
            //CustName = viewer.Name;
            //CustLevelId = viewer.LevelId;
        }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public byte CustLevelId { get; set; }

        //public virtual UserLevel CustLevel => Enumeration.FromValue<UserLevel>(CustLevelId);

        //public int ViewerId => viewer.Id;

        //public string ViewerName => viewer.Name;

        //public byte ViewerLevelId => viewer.LevelId;

        //public virtual UserLevel ActorLevel => Enumeration.FromValue<UserLevel>(ViewerLevelId);

        //public int SubAccountId => viewer.SubAccountId;

        //public string IP => viewer.IP;

        //public string Language => viewer.Language;

        public virtual UserLevel CustLevel => Enumeration.FromValue<UserLevel>(CustLevelId);

        public int ViewerId => 34044212;

        public string ViewerName => "davidtest18feb";

        public byte ViewerLevelId => 4;

        public virtual UserLevel ActorLevel => Enumeration.FromValue<UserLevel>(ViewerLevelId);

        public int SubAccountId => 0;

        public string IP => string.Empty;

        public string Language => "en";

        public virtual bool IsValid() => true;
            //=> CustId > 0
            //&& !string.IsNullOrWhiteSpace(CustName)
            //&& ActorLevel.IsEqualOrUplineOf(CustLevel)
            //&& !string.IsNullOrWhiteSpace(Language);
    }
}