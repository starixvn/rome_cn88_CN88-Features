namespace CN88.Features.Site.Reports.Filters
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Web.Mvc;
    using Alpha.Web.Helpers;
    using Sunplus.Agent.Model.Entities;

    public class ReCheckPasswordAttribute : AuthorizeAttribute
    {
        private const string VerifyPasswordSessionKey = "OldReports.VerifiedPassword";
        private const string VerifiedTimeElapsed = "OldReports.VerifiedTimeElapsed";

        private readonly SessionManager sessionMananger;

        public ReCheckPasswordAttribute()
        {
            sessionMananger = DependencyResolver.Current.GetService<SessionManager>();
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (UserSession.Current?.IsInternal == true)
            {
                return;
            }

            var request = filterContext.RequestContext.HttpContext.Request;
            var isViewBetlistOldReport = request.Params.AllKeys.Contains("IsOldReport", StringComparer.OrdinalIgnoreCase) &&
                bool.Parse(request.Params["IsOldReport"]);
            var isOldReport = !request.Params.AllKeys.Contains("IsOldReport", StringComparer.OrdinalIgnoreCase) &&
                request.Url.PathAndQuery.Contains("OldReports");
            if (isOldReport || isViewBetlistOldReport)
            {
                var elapsed = sessionMananger.Get<DateTime>(VerifiedTimeElapsed);
                elapsed = elapsed == DateTime.MinValue ? DateTime.Now : elapsed;
                var expiredSeconds = int.Parse(ConfigurationManager.AppSettings["OldReports:ExpiredSessionSeconds"]);

                if (!sessionMananger.Get<bool>(VerifyPasswordSessionKey) ||
                    (DateTime.Now - elapsed).TotalSeconds >= expiredSeconds)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.HttpContext.Response.StatusCode = 302;
                        filterContext.HttpContext.Response.End();
                        filterContext.Result = new JsonResult
                        {
                            Data = new { message = "You are not authorized on this function!" },
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("~/OldReports/VerifyPassword");
                    }
                }

                if (Equals(filterContext.RouteData.Values["action"], "VerifyPassword"))
                {
                    filterContext.Result = new RedirectResult("~/OldReports/Index?DisplayMode=1");
                }
            }
        }
    }
}