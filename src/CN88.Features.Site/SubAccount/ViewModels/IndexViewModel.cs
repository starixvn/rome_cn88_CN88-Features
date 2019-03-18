namespace CN88.Features.Site.SubAccount.ViewModels
{
    using System.Collections.Generic;
    using Features.SubAccount.Models;
    using Shared.Helpers;

    public class IndexViewModel
    {
        public IndexViewModel(
            string userCode,
            int pageIndex,
            int pageSize,
            int totalItems,
            IEnumerable<SubAccountItem> subAccounts)
        {
            UserCode = userCode;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalItems = totalItems;
            SubAccounts = subAccounts;
        }

        public string UserCode { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalItems { get; }

        public IEnumerable<SubAccountItem> SubAccounts { get; }

        public string NewSubAccountLink
            => UrlHelper.GetFeaturesAppPath("~/SubAccount/New");

        public string EditSubAccountLink
            => UrlHelper.GetFeaturesAppPath("~/SubAccount/Edit");

        public string DeleteSubAccountLink
            => UrlHelper.GetFeaturesAppPath("~/SubAccount/Delete");

        public string ResetPasswordLink
            => UrlHelper.GetMainAppPath("~/Password/ResetSubAccountPassword");

        public string ResetSecurityCodeLink
            => UrlHelper.GetMainAppPath("~/SecurityCode/ResetSecurityCode");
    }
}