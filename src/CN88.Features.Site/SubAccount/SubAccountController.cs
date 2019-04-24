namespace CN88.Features.Site.SubAccount
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Web.Filters;
    using Features.Models;
    using MediatR;
    using Requests;
    using Shared.Controllers;
    using Web.Filters;

    [RestrictHeadlineFilter]
    public class SubAccountController : FeatureBaseController
    {
        private const string BaseViewPath = "~/SubAccount/Views/{0}.cshtml";

        public SubAccountController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> Index(IndexQuery message)
            => message.IsValid()
                ? View(string.Format(BaseViewPath, nameof(Index)), await mediator.Send(message))
                : View(NoInformationPageViewPath);

        public async Task<ActionResult> New(NewQuery message)
            => message.IsValid()
                ? View(string.Format(BaseViewPath, nameof(New)), await mediator.Send(message))
                : View(NoInformationPageViewPath);

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> New(NewCommand message)
            => Json(
                message.IsValid() ? await mediator.Send(message) : Result.CreateForbiddenResult(),
                JsonRequestBehavior.DenyGet);

        public async Task<ActionResult> Edit(UpdateQuery message)
            => message.IsValid()
                ? View(string.Format(BaseViewPath, nameof(Edit)), await mediator.Send(message))
                : View(NoInformationPageViewPath);

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> Edit(UpdateCommand message)
            => Json(
                message.IsValid() ? await mediator.Send(message) : Result.CreateForbiddenResult(),
                JsonRequestBehavior.DenyGet);
    }
}