namespace CN88.Features.Site.SubAccount
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests;
    using Shared.Controllers;

    public class SubAccountController : FeatureBaseController
    {
        private const string IndexViewPath = "~/SubAccount/Views/Index.cshtml";

        public SubAccountController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> Index(IndexQuery message)
            => await ViewAsync(message, IndexViewPath);
    }
}