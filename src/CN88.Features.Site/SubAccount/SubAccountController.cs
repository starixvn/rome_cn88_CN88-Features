namespace CN88.Features.Site.SubAccount
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Features.Models;
    using MediatR;
    using Requests;
    using Shared.Controllers;

    public class SubAccountController : FeatureBaseController
    {
        private const string IndexViewPath = "~/SubAccount/Views/Index.cshtml";
        private const string CreateViewPath = "~/SubAccount/Views/Create.cshtml";
        private const string UpdateViewPath = "~/SubAccount/Views/Update.cshtml";

        public SubAccountController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> Index(IndexQuery message)
            => message.IsValid()
                ? View(IndexViewPath, await mediator.Send(message))
                : View(NoInformationPageViewPath);

        public async Task<ActionResult> Create(CreateQuery message)
            => message.IsValid()
                ? View(CreateViewPath, await mediator.Send(message))
                : View(NoInformationPageViewPath);

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateCommand message)
        {
            var result = message.IsValid()
                ? await mediator.Send(message)
                : Result.CreateForbiddenResult();

            return View(nameof(Create), new CreateQuery(message.GetViewer()) { Result = result });
        }

        public async Task<ActionResult> Update(UpdateQuery message)
            => message.IsValid()
                ? View(UpdateViewPath, await mediator.Send(message))
                : View(NoInformationPageViewPath);

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(UpdateCommand message)
        {
            var result = message.IsValid()
                ? await mediator.Send(message)
                : Result.CreateForbiddenResult();

            return View(nameof(Update), new UpdateQuery(message.GetViewer()) { Result = result });
        }

        //[HttpPost, AntiXsrfFilter]
        //public async Task<ActionResult> UpdatePermission(UpdatePermissionCommand message)
        //    => await JsonDataAsync(message, JsonRequestBehavior.DenyGet);
    }
}