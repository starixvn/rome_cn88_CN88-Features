namespace CN88.Features.Site.BetLists.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using CN88.BetList.Core.Builders;
    using CN88.Features.BetLists.Constants;
    using CN88.Features.BetLists.Models;
    using CN88.Features.Reports.Constants;
    using CN88.Features.Shared.Configurations;
    using CN88.Features.Shared.Models;
    using CN88.Features.Site.BetLists.Helpers;
    using CN88.Features.Site.BetLists.Requests;
    using CN88.Features.Site.Shared.ViewModels.Components;
    using CN88.Web.Helpers;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;

    public class BetListViewModelBase
    {
        protected static readonly IDictionary<int, int> ReportTypes = new Dictionary<int, int>
        {
            [BetListTypeConstant.Settled] = ReportTypeConstants.SettledWager,
            [BetListTypeConstant.OldSettled] = ReportTypeConstants.SettledWager,
            [BetListTypeConstant.Unsettled] = ReportTypeConstants.UnsettledWager,
            [BetListTypeConstant.Cancelled] = ReportTypeConstants.CancelledReport,
            [BetListTypeConstant.OldCancelled] = ReportTypeConstants.CancelledReport,
            [BetListTypeConstant.Abnormal] = ReportTypeConstants.AbnormalBets,
            [BetListTypeConstant.OldAbnormal] = ReportTypeConstants.AbnormalBets,
        };

        public BetListViewModelBase(
            BetListBaseQuery message,
            BetListData betListData,
            ITicketHelper ticketHelper,
            IBetListCN88HtmlBuilder cn88Builder,
            IBetListHTMLBuilder builder,
            bool isMobile,
            string[] excludingTicketStatus)
        {
            Divisor = message.Divisor;

            CustId = message.CustId;
            CustName = message.CustName;
            CustLevel = message.ActorLevel;
            DisplayVNIP = message.DisplayVNIP;
            ViewerIdOrViewerAdminId = message.ViewerIdOrViewerAdminId;
            ViewerLevelId = message.ViewerLevelId;
            PageIndex = message.PageIndex;
            PageSize = message.PageSize;
            BreadcrumbLevel = message.BreadcrumbLevel;

            BetListData = betListData;
            BetListTotal = new BetListTotal(betListData, Divisor, excludingTicketStatus);
            PagerViewModel = new PagerViewModel(message.PageIndex, message.PageSize, betListData.Tickets.Count());

            TicketHelper = ticketHelper;
            CN88Builder = cn88Builder;
            Builder = builder;
            IsMobile = isMobile;
        }

        public UserLevel CustLevel { get; }

        public string CustName { get; }

        public int NoOfColumns => CalculateNumberOfColumns();

        public BetListData BetListData { get; }

        public BetListTotal BetListTotal { get; }

        public ITicketHelper TicketHelper { get; }

        public IBetListHTMLBuilder Builder { get; }

        public IBetListCN88HtmlBuilder CN88Builder { get; }

        public bool DisplayVNIP { get; }

        public int Divisor { get; }

        public bool IsMobile { get; }

        public bool IsShowPayout { get; protected set; }

        public bool IsShowWinLoss { get; protected set; } = true;

        public decimal CompanyPositionTaking(TicketDto ticket)
            => 1 - ticket.AgentPositionTaking - ticket.MasterPositionTaking - ticket.SuperPositionTaking;

        public int TotalColColspan { get; protected set; } = 2;

        public decimal TotalPayout { get; protected set; }

        public string TotalPayoutText => ConvertByBetType.Winloss(TotalPayout);

        public string OldMainPath => MicroSites.MainUrl();

        public string ResultsPath => AppPathHelper.GetAppPath(AppSettings.ResultAppPath, "~");

        public string TotalBetsForecastPath => AppPathHelper.GetAppPath(AppSettings.TotalBetsForecastAppPath, "~");

        protected virtual int GetMaxColumn() => 9;

        private int CalculateNumberOfColumns()
        {
            var maxColumn = GetMaxColumn();

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

        public int BetTypeGroup { get; protected set; }

        public int CustId { get; }

        public int ViewerIdOrViewerAdminId { get; }

        public byte ViewerLevelId { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public PagerViewModel PagerViewModel { get; }

        public IEnumerable<CustInfo> BreadcrumbLevel { get; }

        public string UserSelectedPeriodId { get; set; }

        public IEnumerable<string> BreadcrumbParamByLevel { get; protected set; }

        public BreadcrumbLevelViewModel BreadcrumbLevelViewModel
            => new BreadcrumbLevelViewModel(BuildBreadcrumbUrl(), GetBreadcrumbLevel());

        public virtual string BuildBreadcrumbUrl() => string.Empty;

        public virtual IEnumerable<CustInfo> GetBreadcrumbLevel()
        {
            var breadcrumbLevel = new List<CustInfo>();

            if (BreadcrumbLevel != null
                && BreadcrumbLevel.Any())
            {
                foreach (var custInfo in BreadcrumbLevel)
                {
                    breadcrumbLevel.Add(custInfo);
                }
            }

            return breadcrumbLevel;
        }

        public virtual BreadcrumbItem BuildBackLink(string request)
        {
            var links = BreadcrumbLevelViewModel
                .BuildBreadcrumbLinks(request, false, BreadcrumbParamByLevel)
                .ToList();

            if (links.Count < 2)
            {
                return new BreadcrumbItem("Not Found", "#");
            }

            return new BreadcrumbItem(CustName, links[links.Count - 2].Link);
        }

        public string GetTotalClass()
        {
            var rowClass = "";
            rowClass += IsAllAbnormal() ? " row-abnormal" : string.Empty;
            rowClass += IsAllInvalidTicket() ? " row-invalid" : string.Empty;
            return rowClass;
        }

        public bool IsAllAbnormal()
            => !BetListData.Tickets.Any(t => !BetListHelpers.IsAbnormalBet(t));

        public bool IsAllInvalidTicket()
            => !BetListData.Tickets.Any(t => !BetListHelpers.IsInvalidTicket(t));
    }
}
