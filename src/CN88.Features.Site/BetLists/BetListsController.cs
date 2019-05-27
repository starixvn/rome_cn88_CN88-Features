namespace CN88.Features.Site.BetLists
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using CN88.Web.Helpers;
    using MediatR;
    using Reports.Filters;
    using Requests;
    using Shared.Controllers;
    using Web.Filters;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class BetListsController : FeatureBaseController
    {
        private readonly BrowserHelper browserHelper;

        public BetListsController(
            IMediator mediator,
            BrowserHelper browserHelper)
            : base(mediator)
        {
            this.browserHelper = browserHelper;
        }

        [HttpGet, ReCheckPassword]
        public async Task<ActionResult> BetListOfReport(BetListOfReportQuery message)
            => browserHelper.IsMobile()
                ? await ViewAsync(message, "ReportBetListMobile")
                : await ViewAsync(message, "ReportBetList");

        [HttpGet, RestrictHeadlineFilter]
        public async Task<ActionResult> BetListOfStatement(BetListOfStatementQuery message)
             => browserHelper.IsMobile()
                ? await ViewAsync(message, "StatementBetListMobile")
                : await ViewAsync(message, "StatementBetList");

        [HttpGet]
        public async Task<ActionResult> ParlaySubTickets(GetParlaySubTicketsQuery message)
            => await ViewAsync(message);

        [HttpGet]
        public async Task<ActionResult> ParlayDetail(GetParlayDetailQuery message)
            => await ViewAsync(message);

        public async Task<ActionResult> ParlayDetailExcel(GetParlayDetailExcelQuery message)
            => await ExcelView(message);

        [ReCheckPassword]
        public async Task<FileContentResult> BetListOfReportExcel(BetListOfReportExcelQuery message)
            => await ExcelView(message);

        [HttpGet, RestrictHeadlineFilter]
        public async Task<ActionResult> OutrightWagerByAccount(OutrightWagerByAccountQuery message)
             => await ViewAsync(message);

        [HttpGet, RestrictHeadlineFilter]
        public async Task<ActionResult> OutrightWagerDetails(OutrightWagerDetailsQuery message)
             => await ViewAsync(message);

        [HttpGet, RestrictHeadlineFilter]
        public async Task<ActionResult> EventWagerByAccount(EventWagerByAccountQuery message)
            => await ViewAsync(message);

        [HttpGet, RestrictHeadlineFilter]
        public async Task<ActionResult> EventWagerDetails(EventWagerDetailsQuery message)
             => await ViewAsync(message);

        [HttpGet, RestrictHeadlineFilter]
        public async Task<ActionResult> StartedWagerByAccount(StartedWagerByAccountQuery message)
             => await ViewAsync(message);

        [HttpGet, RestrictHeadlineFilter]
        public async Task<ActionResult> StartedWagerDetails(StartedWagerDetailsQuery message)
             => await ViewAsync(message);

        [HttpGet, RestrictHeadlineFilter]
        public async Task<ActionResult> TotalBetParlay(TotalBetParlayQuery message)
             => await ViewAsync(message);
    }
}