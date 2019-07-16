namespace CN88.Features.Site.SubAccount
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Web.Filters;
    using MediatR;
    using Requests;
    using Shared.Controllers;
    using Web.Filters;

    [RestrictHeadlineFilter]
    public class SubAccountController : FeatureBaseController
    {
        public SubAccountController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> Index(IndexQuery message)
            => await ViewAsync(message);

        public async Task<ActionResult> New(NewQuery message)
        {
            ViewData["ActorLevel"] = message.ActorLevel;
            return await ViewAsync(message);
        }

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> New(NewCommand message)
            => await JsonDataAsync(message);

        public async Task<ActionResult> Edit(UpdateQuery message)
        {
            ViewData["ActorLevel"] = message.ActorLevel;
            return await ViewAsync(message);
        }

        [HttpPost, AntiXsrfFilter, CheckSubAccBelongingFilter]
        public async Task<ActionResult> Edit(UpdateCommand message)
            => await JsonDataAsync(message);

        [HttpPost, AntiXsrfFilter, CheckSubAccBelongingFilter]
        public async Task<ActionResult> Unlock(UnlockCommand message)
            => await JsonDataAsync(message);
    }
}