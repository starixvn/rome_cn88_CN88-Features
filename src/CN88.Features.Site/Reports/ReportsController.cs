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
        private const string WagerViewPath = "~/Reports/Views/Wager.cshtml";
        private const string LegacyViewPath = "~/Reports/Views/Legacy.cshtml";

        public ReportsController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> Index(IndexQuery message)
            => await ViewAsync(message, MainViewPath);

        public async Task<ActionResult> Wager(ReportQuery message)
            => await ViewAsync(message, WagerViewPath);

        public async Task<ActionResult> Legacy(ReportQuery message)
            => await ViewAsync(message, LegacyViewPath);

        public async Task<ActionResult> Excel(ReportExcelQuery message)
            => await ExcelView(message);

        public async Task<ActionResult> PTHistories(PTHistoryQuery message)
            => await ViewAsync(message, LegacyViewPath);
    }
}