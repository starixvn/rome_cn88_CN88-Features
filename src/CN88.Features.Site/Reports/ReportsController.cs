namespace CN88.Features.Site.Reports
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using CN88.Features.Site.Shared.Helpers;
    using MediatR;
    using Requests;
    using Shared.Controllers;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ReportsController : FeatureBaseController
    {
        private readonly BrowserHelper browserHelper;

        public ReportsController(
            IMediator mediator,
            BrowserHelper browserHelper)
            : base(mediator)
        {
            this.browserHelper = browserHelper;
        }

        public async Task<ActionResult> Index(IndexQuery message)
            => browserHelper.IsMobile()
                ? await ViewAsync(message, "IndexMobile")
                : await ViewAsync(message);

        public async Task<ActionResult> SettlementSummary(SettlementSummaryQuery message)
            => await ViewAsync(message);

        public async Task<ActionResult> AccountingSheet(AccountingSheetQuery message)
            => await ViewAsync(message);

        public async Task<ActionResult> Wager(ReportQuery message)
        {
            message.CurrentUrl = Request.Url.Query;

            return browserHelper.IsMobile()
                ? await ViewAsync(message, "WagerMobile")
                : await ViewAsync(message);
        }

        public async Task<ActionResult> Legacy(ReportQuery message)
            => await ViewAsync(message);

        public async Task<ActionResult> Excel(ReportExcelQuery message)
            => await ExcelView(message);

        public async Task<ActionResult> PTHistory(PTHistoryQuery message)
            => await ViewAsync(message);
    }
}
