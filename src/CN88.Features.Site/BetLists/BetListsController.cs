namespace CN88.Features.Site.BetLists
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests;
    using Shared.Controllers;
    using System.Web.SessionState;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class BetListsController : FeatureBaseController
    {
        public BetListsController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult> BetListOfReport(BetListOfReportQuery message)
            => await ViewAsync(message, "~/BetLists/Views/BetListOfReport.cshtml");

        [HttpGet]
        public async Task<ActionResult> ParlaySubTickets(GetParlaySubTicketsQuery message)
            => await ViewAsync(message, "~/BetLists/Views/ParlaySubTickets.cshtml");

        [HttpGet]
        public async Task<ActionResult> ParlayDetail(GetParlayDetailQuery message)
            => await ViewAsync(message, "~/BetLists/Views/ParlayDetail.cshtml");

        public async Task<ActionResult> ParlayDetailExcel(GetParlayDetailExcelQuery message)
            => await ExcelView(message);

        public async Task<FileContentResult> BetListOfReportExcel(BetListOfReportExcelQuery message)
            => await ExcelView(message);
    }
}