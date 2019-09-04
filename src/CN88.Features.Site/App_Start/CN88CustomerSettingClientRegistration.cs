namespace Alpha.CN88.Site.App_Start
{
    using System;
    using System.Text.RegularExpressions;
    using System.Web;
    using Fanex.Common;
    using global::CN88.CustomerSetting.Client;
    using global::CN88.CustomerSetting.Client.Models;
    using global::CN88.Features.Site.Shared.Helpers;
    using global::CN88.Web.Filters;
    using Sunplus.Agent.Model.BL;
    using Sunplus.Agent.Model.Entities.Permission;

    [NoCacheFilter, AfterLoginFlowFilter]
    public class CN88CustomerSettingClientRegistration : CN88CustomerSettingRegistration
    {
        private const string productIdPattern = "(\"productId\":)([^\r\n][0-9]*)";
        private const int producIdRegexIndex = 2;

        public override bool VerifyQueryPermission(PermissionContext permissionContext)
            => HasPermissionOnProductId(permissionContext);

        public override bool VerifyUpdatePermission(PermissionContext permissionContext)
            => IsAllowPermissionBasedOnSubAccAndInternalAcc() && HasPermissionOnProductId(permissionContext);

        public override bool VerifyMultipleUpdatePermission(PermissionContext permissionContext)
            => IsAllowPermissionBasedOnSubAccAndInternalAcc() && HasPermissionOnProductId(permissionContext);

        public override SettingContext FetchSettingContext()
        {
            var currentUserSession = UserSessionService.GetUserSessionInstance(HttpContext.Current.Session.SessionID);

            return new SettingContext
            {
                ActorId = currentUserSession.CustId,
                ActorName = currentUserSession.UserName,
                ActorLevel = currentUserSession.RoleId,
                SubAccountId = currentUserSession.SubAccId,
                Language = currentUserSession.Language,
                Domain = HttpContext.Current.Request.Url.Host,
                IP = IPHelper.CustomerIP()
            };
        }

        private static bool IsAllowPermissionBasedOnSubAccAndInternalAcc()
        {
            var currentUserSession = UserSessionService.GetUserSessionInstance(HttpContext.Current.Session.SessionID);
            bool subAccountPermission = !currentUserSession.IsSubAcc || currentUserSession.SubPageAccess.UpdateAccount;

            return !currentUserSession.IsInternal && subAccountPermission;
        }

        private static bool HasPermissionOnProductId(PermissionContext permissionContext)
        {
            var productId = GetProductId(permissionContext);

            return productId == 0
                || (UserPermission.HasPermissionOnProduct(productId) && AllowSubAccountToAccessPage());
        }

        private static int GetProductId(PermissionContext permissionContext)
        {
            var queryString = permissionContext.AuthorizationContext.Controller.ValueProvider.GetValue("productId");
            var productId = queryString != null ? Converter.ToInt(queryString.AttemptedValue) : 0;
            if (productId == 0)
            {
                productId = GetProductIdFromJson(permissionContext);
            }

            return productId;
        }

        private static int GetProductIdFromJson(PermissionContext permissionContext)
        {
            var productId = 0;
            var setting = permissionContext.AuthorizationContext.Controller.ValueProvider.GetValue("setting");
            var settingJson = setting != null ? setting.AttemptedValue : string.Empty;

            var regexMatch = Regex.Match(settingJson, productIdPattern, RegexOptions.IgnoreCase);
            if (regexMatch.Success)
            {
                productId = Converter.ToInt(Regex.Replace(regexMatch.Groups[producIdRegexIndex].ToString(), @"[^a-zA-Z0-9_.]+", string.Empty, RegexOptions.Compiled));
            }

            return productId;
        }

        private static bool AllowSubAccountToAccessPage()
        {
            var httpContext = HttpContext.Current;
            var currentUserSession = UserSessionService.GetUserSessionInstance(httpContext.Session.SessionID);

            if (currentUserSession.IsSubAcc)
            {
                var httpRequest = httpContext.Request;

                if (httpRequest.Url.AbsolutePath.EndsWith(
                    "/CN88CustomerSetting/ViewCN88CustomerSetting/Edit",
                    StringComparison.OrdinalIgnoreCase))
                {
                    var custIdQueryString = httpRequest["customerid"];

                    return !string.IsNullOrWhiteSpace(custIdQueryString)
                        && int.TryParse(custIdQueryString, out var custid)
                        && custid > 0;
                }
            }

            return true;
        }
    }
}
