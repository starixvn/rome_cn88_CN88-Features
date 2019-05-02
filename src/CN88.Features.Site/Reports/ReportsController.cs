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
        private const string MainViewPath = "~/Reports/Views/Index.cshtml";
        private const string MainMobileViewPath = "~/Reports/Views/IndexMobile.cshtml";
        private const string WagerViewPath = "~/Reports/Views/Wager.cshtml";
        private const string LegacyViewPath = "~/Reports/Views/Legacy.cshtml";
        private const string PTHistoryViewPath = "~/Reports/Views/PTHistory.cshtml";
        private const string SettlementSummaryPath = "~/Reports/Views/Index/SettlementSummary.cshtml";
        private const string AccountingSheetPath = "~/Reports/Views/Index/AccountingSheet.cshtml";
        private const string ViewBetTypePath = "~/Reports/Views/Bettype.cshtml";
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
                ? await ViewAsync(message, MainMobileViewPath)
                : await ViewAsync(message, MainViewPath);

        public async Task<ActionResult> SettlementSummary(SettlementSummaryQuery message)
            => await ViewAsync(message, SettlementSummaryPath);

        public async Task<ActionResult> AccountingSheet(AccountingSheetQuery message)
            => await ViewAsync(message, AccountingSheetPath);

        public async Task<ActionResult> Wager(ReportQuery message)
            => await ViewAsync(message, WagerViewPath);

        public async Task<ActionResult> Legacy(ReportQuery message)
            => await ViewAsync(message, LegacyViewPath);

        public async Task<ActionResult> Excel(ReportExcelQuery message)
            => await ExcelView(message);

        public async Task<ActionResult> PTHistory(PTHistoryQuery message)
            => await ViewAsync(message, PTHistoryViewPath);

        public async Task<ActionResult> BetType(BettypeQuery message)
            => await ViewAsync(message, ViewBetTypePath);
    }
}