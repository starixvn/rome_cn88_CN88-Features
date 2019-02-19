namespace CN88.Features.Site.Shared.Controllers
{
    using Alpha.Web;
    using CN88.Features.Models;
    using CN88.Features.Site.Requests;
    using MediatR;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public abstract class FeatureBaseController : Controller
    {
        private const string NoInformationPageViewPath = "~/Shared/Views/NoInformation.cshtml";

        protected readonly IMediator mediator;

        protected FeatureBaseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        protected async Task<ActionResult> ViewAsync<TResponse>(IRequest<TResponse> message, string viewPath)
             => (message as BaseQuery).IsValid() ?
                View(viewPath, await mediator.Send(message).ConfigureAwait(false)) : View(NoInformationPageViewPath);

        protected ActionResult JsonData<TResponse>(
           IRequest<TResponse> message,
           JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.AllowGet)
        {
            if (!(message as BaseQuery).IsValid())
            {
                return Json(Result.CreateForbiddenResult());
            }

            return Json(mediator.Send(message), jsonRequestBehavior);
        }

        protected async Task<ActionResult> JsonDataAsync<TResponse>(
            IRequest<TResponse> message,
            JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.AllowGet)
        {
            if (!(message as BaseQuery).IsValid())
            {
                return Json(Result.CreateForbiddenResult());
            }

            return Json(await mediator.Send(message).ConfigureAwait(false), jsonRequestBehavior);
        }
    }
}