namespace CN88.Features.Site.SubAccount
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Web.Filters;
    using MediatR;
    using Requests;
    using Shared.Controllers;

    public class SubAccountController : FeatureBaseController
    {
        private const string IndexViewPath = "~/SubAccount/Views/Index.cshtml";
        private const string CreateViewPath = "~/SubAccount/Views/_Create.cshtml";
        private const string UpdateViewPath = "~/SubAccount/Views/_Update.cshtml";

        public SubAccountController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> Index(IndexQuery message)
            => await ViewAsync(message, IndexViewPath);

        public async Task<ActionResult> Create(CreateQuery message)
            => PartialView(CreateViewPath, await mediator.Send(message));

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> Create(CreateCommand message)
            => await JsonDataAsync(message, JsonRequestBehavior.DenyGet);

        public async Task<ActionResult> Update(UpdateQuery message)
            => await ViewAsync(message, UpdateViewPath);

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> Update(UpdateCommand message)
            => await JsonDataAsync(message, JsonRequestBehavior.DenyGet);

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> UpdatePermission(UpdatePermissionCommand message)
            => await JsonDataAsync(message, JsonRequestBehavior.DenyGet);
    }
}