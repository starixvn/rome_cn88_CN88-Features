namespace CN88.Features.Site.Reports.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests;
    using Shared.Controllers;

    public class MainController : FeatureBaseController
    {
        private const string MainViewPath = "~/Reports/Main/Views/Index.cshtml";

        public MainController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> Index(MainQuery message)
            => await View(message, MainViewPath);
    }
}