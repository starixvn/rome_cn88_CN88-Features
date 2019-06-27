namespace CN88.Features.Site.SubAccount.ViewModels
{
    using System.Collections.Generic;
    using CN88.Web.Helpers;
    using Features.SubAccount.Models;
    using Shared.Helpers;

    public class IndexViewModel
    {
        private readonly BrowserHelper browserHelper;

        public IndexViewModel(
            IEnumerable<SubAccountItem> subAccounts,
            bool viewerIsActive,
            BrowserHelper browserHelper)
        {
            SubAccounts = subAccounts;
            this.browserHelper = browserHelper;
            ViewerIsActive = viewerIsActive;
        }

        public IEnumerable<SubAccountItem> SubAccounts { get; }

        public bool ViewerIsActive { get; }

        public string NewSubAccountLink
            => UrlHelper.GetFeaturesAppPath("~/SubAccount/New");

        public string EditSubAccountLink
            => UrlHelper.GetFeaturesAppPath("~/SubAccount/Edit");

        public string ResetPasswordLink
            => UrlHelper.GetMainAppPath("~/Password/ResetSubAccountPassword");

        public string ResetSecurityCodeLink
            => UrlHelper.GetMainAppPath("~/SecurityCode/ResetSubAccountSecurityCode");

        public bool IsDevice => browserHelper.IsMobile() && !browserHelper.IsTablet();
        
        public string UnlockSubAccountLink
            => UrlHelper.GetFeaturesAppPath("~/SubAccount/Unlock");

        public string VerifySecurityCodeLink
            => UrlHelper.GetMainAppPath("~/SecurityCode/VerifySecurityCode");
    }
}
