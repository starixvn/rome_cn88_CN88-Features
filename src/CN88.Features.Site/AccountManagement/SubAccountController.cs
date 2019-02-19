namespace CN88.Features.Site.AccountManagement
{
    using System.Web.Mvc;
    using MediatR;
    using Shared.Controllers;

    public class SubAccountController : FeatureBaseController
    {
        public SubAccountController(IMediator mediator)
            : base(mediator)
        {
        }

        public ActionResult Index()
        {
            return Content("Sub Account Page");
        }
    }
}