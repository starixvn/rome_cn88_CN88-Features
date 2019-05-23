namespace CN88.Features.Site.BetLists
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using CN88.Web.Helpers;
    using MediatR;
    using Requests.WagerMonitoring;
    using Shared.Controllers;
    using Web.Filters;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class WagerMonitoringController : FeatureBaseController
    {
        private readonly BrowserHelper browserHelper;

        public WagerMonitoringController(IMediator mediator, BrowserHelper browserHelper)
            : base(mediator)
        {
            this.browserHelper = browserHelper;
        }

        [HttpGet, RestrictHeadlineFilter]
        public async Task<ActionResult> Index(WagerMonitoringQuery message)
            => browserHelper.IsMobile()
                ? await ViewAsync(message, "IndexMobile")
                : await ViewAsync(message);

        [HttpPost, RestrictHeadlineFilter]
        public async Task<ActionResult> FilterModal(WagerMonitoringFilterQuery message)
            => await ViewAsync(message);

        [HttpPost, RestrictHeadlineFilter]
        public async Task<ActionResult> LastBets(WagerMonitoringQuery message)
            => await ViewAsync(message);

        [HttpGet, RestrictHeadlineFilter]
        public ActionResult Pause()
            => View("~/BetLists/Views/WagerMonitoring/Pause.cshtml");

        protected override string FeatureFolder => "~/BetLists/Views/WagerMonitoring/";
    }
}