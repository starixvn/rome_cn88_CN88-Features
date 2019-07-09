namespace CN88.Features.Site.Reports
{
    using Alpha.Web.Helpers;
    using CN88.Web.Helpers;
    using Features.Models;
    using Filters;
    using MediatR;
    using Requests;
    using Shared.Controllers;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class OldReportsController : FeatureBaseController
    {
        private const string OldIndexViewPath = "OldIndex";
        private const string OldIndexMobileViewPath = "OldIndexMobile";
        private const string VerifyPasswordViewPath = "VerifyPassword";
        private const string WagerViewPath = "OldWager";
        private const string WagerMobileViewPath = "OldWagerMobile";
        private const string LegacyViewPath = "OldLegacy";
        private const string VerifyPasswordSessionKey = "OldReports.VerifiedPassword";
        private const string VerifiedTimeElapsed = "OldReports.VerifiedTimeElapsed";

        private readonly SessionManager sessionManager;
        private readonly BrowserHelper browserHelper;

        protected override string FeatureFolder => "~/Reports/Views/";

        public OldReportsController(
            IMediator mediator,
            SessionManager sessionManager,
            BrowserHelper browserHelper)
            : base(mediator)
        {
            this.sessionManager = sessionManager;
            this.browserHelper = browserHelper;
        }

        public async Task<ActionResult> VerifyPassword(VerifyPasswordQuery message)
            => await ViewAsync(message);

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPassword(VerifyPasswordCommand message)
        {
            var result = await mediator.Send(message);

            sessionManager.Add(VerifyPasswordSessionKey, result.IsSuccess);
            Checkin();

            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index), new { DisplayMode = 1 });
            }

            return await ViewAsync(
                new VerifyPasswordQuery(DependencyResolver.Current.GetService<IUser>()) { Result = result },
                VerifyPasswordViewPath);
        }

        [ReCheckPassword]
        public async Task<ActionResult> Index(OldIndexQuery message)
            => await ViewAsync(
                message,
                browserHelper.IsMobile()
                ? OldIndexMobileViewPath
                : OldIndexViewPath);

        [ReCheckPassword]
        public Task<ActionResult> Wager(OldReportQuery message)
        {
            message.CurrentUrl = Request.Url.Query;

            return ViewAsync(
                message,
                browserHelper.IsMobile()
                ? WagerMobileViewPath
                : WagerViewPath);
        }

        [ReCheckPassword]
        public async Task<ActionResult> Legacy(OldReportQuery message)
            => await ViewAsync(message, LegacyViewPath);

        [ReCheckPassword]
        public async Task<ActionResult> Excel(OldReportExcelQuery message)
            => await ExcelView(message);

        public bool Checkin()
        {
            sessionManager.Add(VerifiedTimeElapsed, DateTime.Now);
            return true;
        }
    }
}