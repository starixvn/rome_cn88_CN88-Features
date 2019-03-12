namespace CN88.Features.Site.BetLists
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using MediatR;
    using Requests;
    using Shared.Controllers;
    using Reports.Filters;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class BetListsController : FeatureBaseController
    {
        public BetListsController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet, ReCheckPassword]
        public async Task<ActionResult> BetListOfReport(BetListOfReportQuery message)
            => await ViewAsync(message, "~/BetLists/Views/BetListOfReport.cshtml");

        [HttpGet]
        public async Task<ActionResult> BetListOfStatement(BetListOfStatementQuery message)
            => await ViewAsync(message, "~/BetLists/Views/BetListOfStatement.cshtml");

        [HttpGet]
        public async Task<ActionResult> ParlaySubTickets(GetParlaySubTicketsQuery message)
            => await ViewAsync(message, "~/BetLists/Views/ParlaySubTickets.cshtml");

        [HttpGet]
        public async Task<ActionResult> ParlayDetail(GetParlayDetailQuery message)
            => await ViewAsync(message, "~/BetLists/Views/ParlayDetail.cshtml");

        public async Task<ActionResult> ParlayDetailExcel(GetParlayDetailExcelQuery message)
            => await ExcelView(message);

        [ReCheckPassword]
        public async Task<FileContentResult> BetListOfReportExcel(BetListOfReportExcelQuery message)
            => await ExcelView(message);

        [HttpGet]
        public async Task<ActionResult> BetListByAccount(BetListByAccountQuery message)
            => await ViewAsync(message, "~/BetLists/Views/BetListByAccount.cshtml");

        [HttpGet]
        public async Task<ActionResult> WagerDetails(WagerDetailsQuery message)
            => await ViewAsync(message, "~/BetLists/Views/WagerDetails.cshtml");
    }
}