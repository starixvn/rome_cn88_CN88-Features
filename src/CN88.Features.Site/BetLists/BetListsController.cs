namespace CN88.Features.Site.BetLists
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests;
    using Shared.Controllers;

    public class BetListsController : FeatureBaseController
    {
        public BetListsController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult> BetListOfReport(BetListOfReportQuery message)
            => await ViewAsync(message, "~/BetLists/Views/BetListOfReport.cshtml");
    }
}