namespace CN88.Features.Site.SubAccount.ViewModels
{
    using System.Collections.Generic;
    using Alpha.Auth.Configurations;
    using Alpha.Web.Helpers;
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

        public string FeaturesAppPath
            => UrlHelper.GetFeaturesAppPath("~");

        public string ResetPasswordLink
            => BuildFullMainAppPath("Password/ResetSubAccountPassword");

        public string ResetSecurityCodeLink
            => BuildFullMainAppPath("SecurityCode/ResetSecurityCode");

        private static string BuildFullMainAppPath(string url)
            => $"{AppPathHelper.GetAppPath(AppSettings.GetConfigValue("MainAppPath"), "~")}{url}";
    }
}