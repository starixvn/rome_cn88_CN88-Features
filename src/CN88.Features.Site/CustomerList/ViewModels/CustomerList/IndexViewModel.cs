namespace CN88.Features.Site.CustomerList.ViewModels
{
    using Alpha.Auth.Configurations;
    using Alpha.Web.Helpers;
    using CN88.Features.Shared.Models;

    public class IndexViewModel
    {
        private IndexQuery query;

        public IndexViewModel(IndexQuery query)
        {
            this.query = query;
        }

        public UserLevel ActorLevel => query.ActorLevel;

        public bool IsInternal => query.IsInternal;

        public bool HasUpdatePermission => query.HasUpdatePermission;

        public bool HasAddNewPermission => query.HasAddNewPermission;

        public string UsernameAutoSuggestionPath => string.Empty;
        //    => MicroSites.OldMainUrl("~/_GlobalResources/Handlers/QueryUserName.ashx");

        public string EditCustomerLink => $"{SiteFeaturePath}CN88CustomerSetting/ViewCN88CustomerSetting/Edit";

        public string CreateCustomerLink => $"{SiteFeaturePath}CN88CustomerSetting/ViewCN88CustomerSetting/Create";

        public string CurrentLink => $"{SiteFeaturePath}CustomerList/Index";

        public string UpdateCreditLink => $"{SiteFeaturePath}CustomerList/CreditPopup";

        public string UpdateStatusLink => $"{SiteFeaturePath}CustomerList/StatusPopup";

        public string ResetPasswordLink
            => $"{AppPathHelper.GetAppPath(AppSettings.GetConfigValue("MainAppPath"), "~")}Password/ResetDownlinePassword";

        //public string AddNewMasterLink
        //    => $"{CreateCustomerLink}?Language={Language}&ProductId=1&CustomerLevel={customerLevel}" +
        //       $"&CustomerId={custId}&IsReadOnly={isReadOnly}&UplineId={uplineId}&ActorLevel={actorLevel}";

        public string Language => query.Language;

        public int ViewerId => query.ViewerId;

        public string DownlineUsername => query.DownlineUsername;

        public string Username => query.Username;

        public byte SearchRoleId => query.SearchRoleId;

        public byte Status => query.Status;

        public int PageSize => query.PageSize;

        public int PageIndex => query.PageIndex;

        private string SiteFeaturePath => AppPathHelper.GetAppPath(AppSettings.GetConfigValue("FeaturesAppPath"), "~");

        //private string BuildAddNewLink(byte actorLevelId)
        //   => $"{CreateCustomerLink}?Language={Language}&ProductId=1&CustomerLevel={customerLevel}" +
        //      $"&CustomerId={custId}&IsReadOnly={isReadOnly}&UplineId={uplineId}&ActorLevel={actorLevelId}";
    }
}