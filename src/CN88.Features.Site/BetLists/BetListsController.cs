namespace CN88.Features.Site.BetLists
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using CN88.Features.Site.Shared.Helpers;
    using MediatR;
    using Reports.Filters;
    using Requests;
    using Shared.Controllers;
    using Web.Filters;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class BetListsController : FeatureBaseController
    {
        private readonly BrowserHelper browserHelper;
        private const string ReportBetListViewPath = "~/BetLists/Views/BetListOfReport.cshtml";
        private const string ReportBetListMobileViewPath = "~/BetLists/Views/ReportBetListMobile.cshtml";

        public BetListsController(
            IMediator mediator,
            BrowserHelper browserHelper)
            : base(mediator)
        {
            this.browserHelper = browserHelper;
        }

        [HttpGet, ReCheckPassword]
        public async Task<ActionResult> BetListOfReport(BetListOfReportQuery message)
            => await ViewAsync(
                message, 
                browserHelper.IsMobile() 
                    ? ReportBetListMobileViewPath
                    : ReportBetListViewPath);

        [HttpGet, RestrictHeadlineFilter]
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

        [HttpGet, RestrictHeadlineFilter]
        public async Task<ActionResult> OutrightWagerByAccount(OutrightWagerByAccountQuery message)
            => await ViewAsync(message, "~/BetLists/Views/OutrightWagerByAccount.cshtml");

        [HttpGet, RestrictHeadlineFilter]
        public async Task<ActionResult> OutrightWagerDetails(OutrightWagerDetailsQuery message)
            => await ViewAsync(message, "~/BetLists/Views/OutrightWagerDetails.cshtml");

        [HttpGet, RestrictHeadlineFilter]
        public async Task<ActionResult> EventWagerByAccount(EventWagerByAccountQuery message)
            => await ViewAsync(message, "~/BetLists/Views/EventWagerByAccount.cshtml");

        [HttpGet, RestrictHeadlineFilter]
        public async Task<ActionResult> EventWagerDetails(EventWagerDetailsQuery message)
            => await ViewAsync(message, "~/BetLists/Views/EventWagerDetails.cshtml");

        [HttpGet, RestrictHeadlineFilter]
        public async Task<ActionResult> StartedWagerByAccount(StartedWagerByAccountQuery message)
            => await ViewAsync(message, "~/BetLists/Views/StartedWagerByAccount.cshtml");

        [HttpGet, RestrictHeadlineFilter]
        public async Task<ActionResult> StartedWagerDetails(StartedWagerDetailsQuery message)
            => await ViewAsync(message, "~/BetLists/Views/StartedWagerDetails.cshtml");

        [HttpGet, RestrictHeadlineFilter]
        public async Task<ActionResult> TotalBetParlay(TotalBetParlayQuery message)
            => await ViewAsync(message, "~/BetLists/Views/TotalBetParlay.cshtml");
    }
}