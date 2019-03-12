namespace CN88.Features.Site.Shared.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Features.Models;
    using MediatR;
    using Requests;
    using ViewModels;
    using Web.Controllers;

    public abstract class FeatureBaseController : CN88BaseController
    {
        private const string NoInformationPageViewPath = "~/Shared/Views/NoInformation.cshtml";
        private const string InvalidExcelFileName = "Invalid";

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

        protected async Task<FileContentResult> ExcelView(IRequest<ExcelFileViewModel> message)
        {
            var baseQuery = message as BaseQuery;
            if (baseQuery != null && !baseQuery.IsValid())
            {
                return GetExcelContent(new ExcelFileViewModel(InvalidExcelFileName, new byte[] { }));
            }

            return GetExcelContent(await mediator.Send(message));
        }

        private FileContentResult GetExcelContent(ExcelFileViewModel viewModel)
            => File(viewModel.Content, viewModel.ContentType, viewModel.Name);
    }
}