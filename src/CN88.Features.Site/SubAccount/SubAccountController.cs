namespace CN88.Features.Site.SubAccount
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Web.Filters;
    using Features.Models;
    using MediatR;
    using Requests;
    using Shared.Controllers;

    public class SubAccountController : FeatureBaseController
    {
        private const string IndexViewPath = "~/SubAccount/Views/Index.cshtml";
        private const string NewViewPath = "~/SubAccount/Views/New.cshtml";
        private const string EditViewPath = "~/SubAccount/Views/Edit.cshtml";

        public SubAccountController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> Index(IndexQuery message)
            => message.IsValid()
                ? View(IndexViewPath, await mediator.Send(message))
                : View(NoInformationPageViewPath);

        public async Task<ActionResult> New(NewQuery message)
            => message.IsValid()
                ? View(NewViewPath, await mediator.Send(message))
                : View(NoInformationPageViewPath);

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> New(NewCommand message)
            => Json(
                message.IsValid() ? await mediator.Send(message) : Result.CreateForbiddenResult(),
                JsonRequestBehavior.DenyGet);

        public async Task<ActionResult> Edit(UpdateQuery message)
            => message.IsValid()
                ? View(EditViewPath, await mediator.Send(message))
                : View(NoInformationPageViewPath);

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> Edit(UpdateCommand message)
            => Json(
                message.IsValid() ? await mediator.Send(message) : Result.CreateForbiddenResult(),
                JsonRequestBehavior.DenyGet);

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> Delete(DeleteCommand message)
            => Json(
                message.IsValid() ? await mediator.Send(message) : Result.CreateForbiddenResult(),
                JsonRequestBehavior.DenyGet);
    }
}