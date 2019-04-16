namespace CN88.Features.Site.SubAccount.ViewModels
{
    using System.Collections.Generic;
    using Features.SubAccount.Models;
    using Shared.Helpers;

    public class IndexViewModel
    {
        public IndexViewModel(IEnumerable<SubAccountItem> subAccounts)
        {
            SubAccounts = subAccounts;
        }

        public IEnumerable<SubAccountItem> SubAccounts { get; }

        public string NewSubAccountLink
            => UrlHelper.GetFeaturesAppPath("~/SubAccount/New");

        public string EditSubAccountLink
            => UrlHelper.GetFeaturesAppPath("~/SubAccount/Edit");

        public string ResetPasswordLink
            => UrlHelper.GetMainAppPath("~/Password/ResetSubAccountPassword");

        public string ResetSecurityCodeLink
            => UrlHelper.GetMainAppPath("~/SecurityCode/ResetSubAccountSecurityCode");
    }
}