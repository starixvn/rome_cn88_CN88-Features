namespace CN88.Features.Site.Reports
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Web.Helpers;
    using Filters;
    using MediatR;
    using Requests;
    using Shared.Controllers;
    using System;
    using Features.Models;
    using CN88.Features.Site.Shared.Helpers;

    public class OldReportsController : FeatureBaseController
    {
        private const string OldIndexViewPath = "~/Reports/Views/OldIndex.cshtml";
        private const string OldIndexMobileViewPath = "~/Reports/Views/OldIndexMobile.cshtml";
        private const string VerifyPasswordViewPath = "~/Reports/Views/VerifyPassword.cshtml";
        private const string WagerViewPath = "~/Reports/Views/OldWager.cshtml";
        private const string WagerMobileViewPath = "~/Reports/Views/OldWagerMobile.cshtml";
        private const string LegacyViewPath = "~/Reports/Views/OldLegacy.cshtml";
        private const string VerifyPasswordSessionKey = "OldReports.VerifiedPassword";
        private const string VerifiedTimeElapsed = "OldReports.VerifiedTimeElapsed";

        private readonly SessionManager sessionManager;
        private readonly BrowserHelper browserHelper;

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
            => await ViewAsync(message, VerifyPasswordViewPath);

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
        public async Task<ActionResult> Wager(OldReportQuery message)
            => await ViewAsync(
                message,
                browserHelper.IsMobile()
                ? WagerMobileViewPath
                : WagerViewPath);

        [ReCheckPassword]
        public async Task<ActionResult> Legacy(OldReportQuery message)
            => await ViewAsync(message, LegacyViewPath);

        public bool Checkin()
        {
            sessionManager.Add(VerifiedTimeElapsed, DateTime.Now);
            return true;
        }
    }
}