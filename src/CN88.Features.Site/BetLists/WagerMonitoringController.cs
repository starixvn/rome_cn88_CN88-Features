namespace CN88.Features.Site.BetLists
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using MediatR;
    using Requests.WagerMonitoring;
    using Shared.Controllers;
    using ViewModels.WagerMonitoring;
    using Web.Filters;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class WagerMonitoringController : FeatureBaseController
    {
        public WagerMonitoringController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet, RestrictHeadlineFilter]
        public async Task<ActionResult> Index(WagerMonitoringQuery message)
            => await ViewAsync(message, "~/BetLists/Views/WagerMonitoring/Index.cshtml");

        [HttpGet, RestrictHeadlineFilter]
        public async Task<ActionResult> FilterModal(WagerMonitoringFilterQuery message)
            => await ViewAsync(message, "~/BetLists/Views/WagerMonitoring/FilterModal.cshtml");

        [HttpGet, RestrictHeadlineFilter]
        public async Task<ActionResult> LastBets(WagerMonitoringQuery message)
            => await ViewAsync(message, "~/BetLists/Views/WagerMonitoring/LastBets.cshtml");

        [HttpGet, RestrictHeadlineFilter]
        public ActionResult Pause()
            => View("~/BetLists/Views/WagerMonitoring/Pause.cshtml");
    }
}