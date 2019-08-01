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
            bool showActionButtons,
            bool showAccountManagementPermission,
            BrowserHelper browserHelper)
        {
            SubAccounts = subAccounts;
            this.browserHelper = browserHelper;
            ShowActionButtons = showActionButtons;
            ShowAccountManagementPermission = showAccountManagementPermission;
        }

        public IEnumerable<SubAccountItem> SubAccounts { get; }

        public bool ShowActionButtons { get; }

        public bool ShowAccountManagementPermission { get; }

        public int TotalColumns
        {
            get
            {
                var totalColumns = ShowActionButtons ? 9 : 8;

                return ShowAccountManagementPermission ? totalColumns : (totalColumns - 2);
            }
        }

        public string NewSubAccountLink
            => UrlHelper.GetFeaturesAppPath("~/SubAccount/New");

        public string EditSubAccountLink
            => UrlHelper.GetFeaturesAppPath("~/SubAccount/Edit");

        public string ResetPasswordLink
            => UrlHelper.GetMainAppPath("~/Password/ResetSubAccountPassword");

        public string ResetSecurityCodeLink
            => UrlHelper.GetMainAppPath("~/SecurityCode/ResetSubAccountSecurityCode");

        public bool IsDevice => browserHelper.IsMobile();

        public string UnlockSubAccountLink
            => UrlHelper.GetFeaturesAppPath("~/SubAccount/Unlock");

        public string VerifySecurityCodeLink
            => UrlHelper.GetMainAppPath("~/SecurityCode/VerifySecurityCode");

        public string DeleteSubAccountLink
            => UrlHelper.GetFeaturesAppPath("~/SubAccount/Delete");
    }
}
