namespace CN88.Features.Site.Reports.Filters
{
    using System.Web.Mvc;
    using Alpha.Web.Helpers;
    using System;
    using System.Configuration;

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

            var elapsed = sessionMananger.Get<DateTime>(VerifiedTimeElapsed);
                elapsed = elapsed == DateTime.MinValue ? DateTime.Now : elapsed;
            var expiredSeconds = int.Parse(ConfigurationManager.AppSettings["OldReports:ExpiredSessionSeconds"]);

            if (!sessionMananger.Get<bool>(VerifyPasswordSessionKey) ||
                DateTime.Now.Subtract(elapsed).Seconds >= expiredSeconds)
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
            else if (Equals(filterContext.RouteData.Values["action"], "VerifyPassword"))
            {
                filterContext.Result = new RedirectResult("~/OldReports/Index?DisplayMode=1");
            }
        }
    }
}