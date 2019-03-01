namespace CN88.Features.Site.Reports
{
    using System.Web.Mvc;
    using MediatR;
    using Requests;
    using Shared.Controllers;
    using ViewModels;

    public class OldReportsController : FeatureBaseController
    {
        private const string VerifyPasswordViewPath = "~/Reports/Views/VerifyPassword.cshtml";

        public OldReportsController(IMediator mediator)
            : base(mediator)
        {
        }

        public ActionResult VerifyPassword(VerifyPasswordQuery message)
        {
            return View(VerifyPasswordViewPath, new VerifyPasswordViewModel(message.NickName));
        }
    }
}