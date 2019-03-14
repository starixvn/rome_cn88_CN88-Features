namespace Alpha.CN88.Site.App_Start
{
    using Fanex.Common;
    using global::CN88.CustomerSetting.Client;
    using global::CN88.CustomerSetting.Client.Models;
    using global::CN88.Features.Site.Helpers;
    using Sunplus.Agent.Model.BL;
    using Sunplus.Agent.Model.Entities.Permission;
    using System.Text.RegularExpressions;
    using System.Web;

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
#pragma warning disable CS0618 // Type or member is obsolete
            var currentUserSession = UserSessionService.GetUserSessionInstance(HttpContext.Current.Session.SessionID);
#pragma warning restore CS0618 // Type or member is obsolete
            return new SettingContext()
            {
                ActorId = currentUserSession.UserId,
                ActorName = currentUserSession.UserName,
                ActorLevel = currentUserSession.RoleId,
                SubAccountId = currentUserSession.SubAccId,
                Language = currentUserSession.Language,
                Domain = HttpContext.Current.Request.Url.Host,
                IP = IPHelper.CustomerIp()
            };
        }

        private bool IsAllowPermissionBasedOnSubAccAndInternalAcc()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            var currentUserSession = UserSessionService.GetUserSessionInstance(HttpContext.Current.Session.SessionID);
#pragma warning restore CS0618 // Type or member is obsolete
            bool subAccountPermission = !currentUserSession.IsSubAcc || currentUserSession.SubPageAccess.UpdateAccount;
            return !currentUserSession.IsInternal && subAccountPermission;
        }

        private bool HasPermissionOnProductId(PermissionContext permissionContext)
        {
            var productId = GetProductId(permissionContext);

#pragma warning disable CS0618 // Type or member is obsolete
            return productId == 0 ? true : UserPermission.HasPermissionOnProduct(productId);
#pragma warning restore CS0618 // Type or member is obsolete
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