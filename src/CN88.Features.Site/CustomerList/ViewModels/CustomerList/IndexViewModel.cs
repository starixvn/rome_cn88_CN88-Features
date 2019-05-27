namespace CN88.Features.Site.CustomerList.ViewModels
{
    using Alpha.Auth.Configurations;
    using Alpha.Web.Helpers;
    using CN88.Web.Helpers;
    using Features.Shared.Models;

    public class IndexViewModel
    {
        private readonly IndexQuery query;

        public IndexViewModel(IndexQuery query, BrowserHelper browserHelper)
        {
            this.query = query;
            IsDevice = browserHelper.IsMobile() && !browserHelper.IsTablet();
        }

        public UserLevel ActorLevel => query.ActorLevel;

        public bool IsInternal => query.IsInternal;

        public bool AccountManagementEditView => query.AccountManagementEditView;

        public string UsernameAutoSuggestionPath
            => MicroSites.MainUrl("~/Suggestion/Index");

        public string EditCustomerLink => $"{SiteFeaturePath}CN88CustomerSetting/ViewCN88CustomerSetting/Edit";

        public string CreateCustomerLink => $"{SiteFeaturePath}CN88CustomerSetting/ViewCN88CustomerSetting/Create";

        public string CurrentLink => $"{SiteFeaturePath}CustomerList/Index";

        public string UpdateCreditLink => $"{SiteFeaturePath}CustomerList/CreditPopup";

        public string UpdateStatusLink => $"{SiteFeaturePath}CustomerList/StatusPopup";

        public string ResetPasswordLink
            => $"{SiteMainPath}Password/ResetDownlinePassword";

        public string ResetSecurityCodeLink
            => $"{SiteMainPath}SecurityCode/ResetSecurityCode";

        public string AccessDenyPage
            => $"{SiteMainPath}AccessDenied.aspx";

        public string Language => query.Language;

        public int ViewerId => query.ViewerId;

        public bool IsActiveViewer => query.IsActiveViewer;

        public string DownlineUsername => query.DownlineUsername;

        public string Username => query.Username;

        public byte SearchRoleId => query.SearchRoleId;

        public byte Status => query.Status;

        public int PageSize => query.PageSize;

        public int PageIndex => query.PageIndex;

        private string SiteFeaturePath => AppPathHelper.GetAppPath(AppSettings.GetConfigValue("FeaturesAppPath"), "~");

        private string SiteMainPath => AppPathHelper.GetAppPath(AppSettings.GetConfigValue("MainAppPath"), "~");

        public bool IsDevice { get; }

        public bool IsNotPhone => !IsDevice;
    }
}
