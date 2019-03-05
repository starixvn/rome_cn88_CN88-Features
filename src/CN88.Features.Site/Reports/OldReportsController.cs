namespace CN88.Features.Site.Reports
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using MediatR;
    using Requests;
    using Shared.Controllers;

    public class OldReportsController : FeatureBaseController
    {
        private const string OldIndexViewPath = "~/Reports/Views/OldIndex.cshtml";
        private const string VerifyPasswordViewPath = "~/Reports/Views/VerifyPassword.cshtml";
        private const string WagerViewPath = "~/Reports/Views/OldWager.cshtml";
        private const string LegacyViewPath = "~/Reports/Views/OldLegacy.cshtml";

        public OldReportsController(IMediator mediator)
            : base(mediator)
        {
        }

        public async Task<ActionResult> VerifyPassword(VerifyPasswordQuery message)
            => await ViewAsync(message, VerifyPasswordViewPath);

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPassword(VerifyPasswordCommand message)
        {
            var result = await mediator.Send(message);

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index), new { DisplayMode = 1 });
            }

            return Json(result);
        }

        public async Task<ActionResult> Index(OldIndexQuery message)
            => await ViewAsync(message, OldIndexViewPath);

        public async Task<ActionResult> Wager(OldReportQuery message)
            => await ViewAsync(message, WagerViewPath);

        public async Task<ActionResult> Legacy(OldReportQuery message)
            => await ViewAsync(message, LegacyViewPath);
    }
}