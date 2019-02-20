namespace CN88.Features.Site.CustomerList.ViewModels
{
    using Alpha.Auth.Configurations;
    using Alpha.Web.Helpers;
    using CN88.Features.Shared.Models;

    public class IndexViewModel
    {
        public IndexViewModel(IndexQuery query)
        {
            ActorLevel = query.ActorLevel;
            IsInternal = query.IsInternal;
            HasUpdatePermission = query.HasUpdatePermission;
            HasAddNewPermission = query.HasAddNewPermission;

            Language = query.Language;
            ViewerId = query.ViewerId;

            DownlineUsername = query.DownlineUsername;
            Username = query.Username;
            SearchRoleId = query.SearchRoleId;
            Status = query.Status;
            PageSize = query.PageSize;
            PageIndex = query.PageIndex;
        }

        public UserLevel ActorLevel { get; set; }

        public bool IsInternal { get; set; }

        public bool HasUpdatePermission { get; set; }

        public bool HasAddNewPermission { get; set; }

        public static string UsernameAutoSuggestionPath
            => MicroSites.OldMainUrl("~/_GlobalResources/Handlers/QueryUserName.ashx");

        public static string EditCustomerLink => $"{SiteFeaturePath}/CN88CustomerSetting/ViewCN88CustomerSetting/Edit";

        public static string CreateCustomerLink => $"{SiteFeaturePath}/CN88CustomerSetting/ViewCN88CustomerSetting/Create";

        public static string CurrentLink => MicroSites.CN88Url("~/CustomerList/Index");

        public static string StatusPopupLink => MicroSites.CN88Url("~/CustomerList/StatusPopup");

        public static string CreditPopupLink => MicroSites.CN88Url("~/CustomerList/CreditPopup");

        public string Language { get; }

        public int ViewerId { get; }

        public string DownlineUsername { get; }

        public string Username { get; }

        public byte SearchRoleId { get; }

        public byte Status { get; }

        public int PageSize { get; }

        public int PageIndex { get; }

        private static string SiteFeaturePath => AppPathHelper.GetAppPath(AppSettings.GetConfigValue("FeaturesAppPath"), "~");
    }
}