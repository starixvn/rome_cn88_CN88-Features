namespace CN88.Features.Site.Shared
{
    using System.Web.Mvc;
    using CN88.Web.Controllers;

    public class UserController : CN88BaseController
    {
        public ActionResult AccessDenied()
            => View("~/Shared/Views/User/AccessDenied.cshtml");
    }
}