namespace CN88.Features.Site.Reports
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests;
    using Shared.Controllers;

    public class ReportsController : FeatureBaseController
    {
        private const string MainViewPath = "~/Reports/Views/Index.cshtml";

        public ReportsController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> Index(MainQuery message)
            => await View(message, MainViewPath);

        public async Task<ActionResult> Wager()
            => View();

        public async Task<ActionResult> Lagecy()
            => View();
    }
}