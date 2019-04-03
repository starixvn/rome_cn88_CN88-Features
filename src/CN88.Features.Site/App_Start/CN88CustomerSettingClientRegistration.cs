namespace Alpha.CN88.Site.App_Start
{
    using System.Text.RegularExpressions;
    using System.Web;
    using Fanex.Common;
    using global::CN88.CustomerSetting.Client;
    using global::CN88.CustomerSetting.Client.Models;
    using global::CN88.Features.Site.Shared.Helpers;
    using Sunplus.Agent.Model.BL;
    using Sunplus.Agent.Model.Entities.Permission;
    using global::CN88.Web.Filters;
    using Web.Filters;

    [NoCacheFilter, AfterLoginFlowFilter]
    public class CN88CustomerSettingClientRegistration : CN88CustomerSettingRegistration
    {
        public override bool VerifyQueryPermission(PermissionContext permissionContext)
            => HasPermissionOnProductId(permissionContext);

        public override bool VerifyUpdatePermission(PermissionContext permissionContext)
            => IsAllowPermissionBasedOnSubAccAndInternalAcc() && HasPermissionOnProductId(permissionContext);

        public override bool VerifyMultipleUpdatePermission(PermissionContext permissionContext)
            => IsAllowPermissionBasedOnSubAccAndInternalAcc() && HasPermissionOnProductId(permissionContext);

        public override SettingContext FetchSettingContext()
        {
            var currentUserSession = UserSessionService.GetUserSessionInstance(HttpContext.Current.Session.SessionID);

            return new SettingContext()
            {
                ActorId = currentUserSession.UserId,
                ActorName = currentUserSession.UserName,
                ActorLevel = currentUserSession.RoleId,
                SubAccountId = currentUserSession.SubAccId,
                Language = currentUserSession.Language,
                Domain = HttpContext.Current.Request.Url.Host,
                IP = IPHelper.CustomerIP()
            };
        }

        private bool IsAllowPermissionBasedOnSubAccAndInternalAcc()
        {
            var currentUserSession = UserSessionService.GetUserSessionInstance(HttpContext.Current.Session.SessionID);
            bool subAccountPermission = !currentUserSession.IsSubAcc || currentUserSession.SubPageAccess.UpdateAccount;

            return !currentUserSession.IsInternal && subAccountPermission;
        }

        private bool HasPermissionOnProductId(PermissionContext permissionContext)
        {
            var productId = GetProductId(permissionContext);

            return productId == 0 ? true : UserPermission.HasPermissionOnProduct(productId);
        }

        private int GetProductId(PermissionContext permissionContext)
        {
            var queryString = permissionContext.AuthorizationContext.Controller.ValueProvider.GetValue("productId");
            var productId = queryString != null ? Converter.ToInt(queryString.AttemptedValue) : 0;
            if (productId == 0)
            {
                productId = GetProductIdFromJson(permissionContext);
            }

            return productId;
        }

        private int GetProductIdFromJson(PermissionContext permissionContext)
        {
            var productId = 0;
            var pattern = "(\"productId\":)([^\r\n][0-9]*)";
            var setting = permissionContext.AuthorizationContext.Controller.ValueProvider.GetValue("setting");
            var settingJson = setting != null ? setting.AttemptedValue : string.Empty;

            var regexMatch = Regex.Match(settingJson, pattern, RegexOptions.IgnoreCase);
            if (regexMatch.Success)
            {
                productId = Converter.ToInt(Regex.Replace(regexMatch.Groups[2].ToString(), @"[^a-zA-Z0-9_.]+", string.Empty, RegexOptions.Compiled));
            }

            return productId;
        }
    }
}