namespace CN88.Features.Site.Reports.Filters
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Alpha.Web.Helpers;

    public class ReCheckPasswordAttribute : AuthorizeAttribute
    {
        private const string VerifyPasswordSessionKey = "OldReports.VerifiedPassword";

        private readonly SessionManager sessionMananger;

        public ReCheckPasswordAttribute()
        {
            sessionMananger = DependencyResolver.Current.GetService<SessionManager>();
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (!sessionMananger.Get<bool>(VerifyPasswordSessionKey))
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = 302;
                    filterContext.HttpContext.Response.End();
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                Controller = "OldReports",
                                Action = "VerifyPassword"
                            }));
                }
            }
        }
    }
}