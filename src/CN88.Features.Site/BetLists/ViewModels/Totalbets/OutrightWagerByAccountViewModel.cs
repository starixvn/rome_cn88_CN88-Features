namespace CN88.Features.Site.BetLists.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Web.Helpers;
    using BetList.Core.Builders;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Features.BetLists.Models;
    using Features.Shared.Configurations;
    using Requests;
    using Shared.Helpers;
    using Shared.ViewModels.Components;

    public class OutrightWagerByAccountViewModel : BetListViewModelBase
    {
        protected readonly OutrightWagerByAccountQuery message;

        public OutrightWagerByAccountViewModel(
              OutrightWagerByAccountQuery message,
              BetListData betListData,
              ITicketHelper ticketHelper,
              IBetListCN88HtmlBuilder cn88Builder,
              IBetListHTMLBuilder builder,
              bool isMobile,
              string[] excludingTicketStatus = null)
        {
            Divisor = (message.ActorLevel.IsHeadline()) ? 100 : 1;

            CustLevel = message.ActorLevel;
            DisplayVNIP = message.DisplayVNIP;
            CustName = message.CustName;
            CustId = message.CustId;
            Stake = message.Stake;
            StakeCondition = message.StakeCondition;
            MatchId = message.MatchId;

            NoOfColumns = CalculateNumberOfColumns();

            BetListData = betListData;
            BetListTotal = new BetListTotal(betListData, Divisor, excludingTicketStatus);
            TicketHelper = ticketHelper;
            CN88Builder = cn88Builder;
            Builder = builder;

            ViewerIdOrViewerAdminId = message.ViewerIdOrViewerAdminId;
            ViewerLevelId = message.ViewerLevelId;

            PageIndex = message.PageIndex;
            PageSize = message.PageSize;
            PagerViewModel = new PagerViewModel(message.PageIndex, message.PageSize, betListData.Tickets.Count());

            this.message = message;
            var reportPath = AppPathHelper.GetAppPath(AppSettings.TotalBetAppPath, "~/WagerAccount/GetByOutright");
            BreadcrumbParamByLevel = CalculateBreadcrumbParamByLevel(reportPath);

            IsMobile = isMobile;

            IsShowWinLoss = false;
            BreadcrumbLevel = message.BreadcrumbLevel;
        }

        public decimal Stake { get; }

        public int StakeCondition { get; }

        public int MatchId { get; }

        public string WagerDetailsLink()
        {
            return UrlHelper.ReplaceOrAddQueryStringParamsFunc(
                  AppPathHelper.GetAppPath(AppSettings.FeaturesAppPath, "~/BetLists/OutrightWagerDetails"),
                  new Dictionary<string, object>
                  {
                      ["CustId"] = message.ViewerId,
                      ["CustName"] = message.ViewerName,
                      ["CustLevelId"] = message.ActorLevel.Value,
                      ["MatchId"] = MatchId,
                      ["Stake"] = Stake,
                      ["StakeCondition"] = StakeCondition
                  });
        }

        public override string BuildBreadcrumbUrl()
            => UrlHelper.AddQueryStringParamsFunc(
                AppPathHelper.GetAppPath(AppSettings.TotalBetAppPath, "~/WagerAccount/GetByOutright"),
                new Dictionary<string, object>
                {
                    ["CustId"] = CustId,
                    ["CustName"] = CustName,
                    ["CustLevelId"] = CustLevel.Value,
                    ["MatchId"] = MatchId,
                    ["Stake"] = Stake,
                    ["StakeCondition"] = StakeCondition
                });
    }
}