namespace CN88.Features.Site.BetLists.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Web.Helpers;
    using CN88.BetList.Core.Builders;
    using CN88.Features.BetLists.Models;
    using CN88.Features.Shared.Configurations;
    using CN88.Features.Shared.Models;
    using CN88.Features.Site.Shared.ViewModels.Components;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;

    public class BetListTableViewModel
    {
        public UserLevel CustLevel { get; protected set; }

        public string CustName { get; protected set; }

        public int NoOfColumns { get; protected set; }

        public BetListData BetListData { get; protected set; }

        public BetListTotal BetListTotal { get; protected set; }

        public ITicketHelper TicketHelper { get; protected set; }

        public IBetListHTMLBuilder Builder { get; protected set; }

        public IBetListCN88HtmlBuilder CN88Builder { get; protected set; }

        public bool DisplayVNIP { get; protected set; }

        public int Divisor { get; protected set; }

        public bool IsMobile { get; protected set; }

        public bool IsShowPayout { get; protected set; }

        public bool IsShowWinLoss { get; protected set; } = true;

        public decimal CompanyPositionTaking(TicketDto ticket)
            => 1 - ticket.AgentPositionTaking - ticket.MasterPositionTaking - ticket.SuperPositionTaking;

        public int TotalColColspan { get; protected set; } = 2;

        public decimal TotalPayout { get; protected set; }

        public string TotalPayoutText => ConvertByBetType.Winloss(TotalPayout);

        public string OldMainPath => MicroSites.OldMainUrl("~");

        public string ResultsPath => AppPathHelper.GetAppPath(AppSettings.ResultAppPath, "~");

        public string TotalBetsForecastPath => AppPathHelper.GetAppPath(AppSettings.TotalBetsForecastAppPath, "~");

        protected virtual int CalculateNumberOfColumns(int maxColumn = 9)
        {
            if (CustLevel.IsSuperLevel() || CustLevel.IsHeadline())
            {
                return maxColumn;
            }

            return maxColumn - (CustLevel.IsMasterLevel() ? 2 : 3);
        }

        protected virtual IEnumerable<string> CalculateBreadcrumbParamByLevel(string reportPath)
        {
            if (CustLevel.IsHeadline() || CustLevel.IsSuperLevel())
            {
                return Enumerable.Repeat(reportPath, UserLevel.Super.Value);
            }

            return CustLevel.IsMemberLevel()
                ? Enumerable.Empty<string>()
                : Enumerable.Repeat(reportPath, CustLevel.Value);
        }

        public int SportTypeGroup { get; protected set; }

        public int CustId { get; protected set; }

        public int ViewerIdOrViewerAdminId { get; protected set; }

        public byte ViewerLevelId { get; protected set; }

        public int PageIndex { get; protected set; }

        public int PageSize { get; protected set; }

        public PagerViewModel PagerViewModel { get; protected set; }
    }
}