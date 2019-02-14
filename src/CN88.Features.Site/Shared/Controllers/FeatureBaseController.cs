namespace CN88.Features.Site.Shared.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Web;
    using MediatR;

    public abstract class FeatureBaseController : BaseAsyncController
    {
        protected readonly IMediator mediator;

        protected FeatureBaseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        protected async Task<ActionResult> View<TResponse>(IRequest<TResponse> message, string viewPath)
            => View(viewPath, await mediator.Send(message).ConfigureAwait(false));
    }
}