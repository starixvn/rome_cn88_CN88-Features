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

    public class EventWagerDetailsViewModel : BetListViewModelBase
    {
        public EventWagerDetailsViewModel(
              EventWagerDetailsQuery message,
              BetListData betListData,
              ITicketHelper ticketHelper,
              IBetListCN88HtmlBuilder cn88Builder,
              IBetListHTMLBuilder builder,
              bool isMobile,
              string[] excludingTicketStatus = null)
            : base(message, ticketHelper, cn88Builder, builder)
        {
            Stake = message.Stake;
            StakeCondition = message.StakeCondition;
            MatchId = message.MatchId;
            BetType = message.BetType;
            LiveIndicator = message.LiveIndicator;
            BetTeam = message.BetTeam;

            NoOfColumns = CalculateNumberOfColumns();

            BetListData = betListData;
            BetListTotal = new BetListTotal(betListData, Divisor, excludingTicketStatus);

            PagerViewModel = new PagerViewModel(message.PageIndex, message.PageSize, betListData.Tickets.Count());

            IsShowWinLoss = false;

            IsMobile = isMobile;
            BreadcrumbLevel = message.BreadcrumbLevel;
        }

        public int MatchId { get; }

        public int BetType { get; }

        public int LiveIndicator { get; }

        public string BetTeam { get; }

        public decimal Stake { get; }

        public int StakeCondition { get; }

        public string WagerByAccountLink()
            => UrlHelper.ReplaceOrAddQueryStringParamsFunc(
                  AppPathHelper.GetAppPath(AppSettings.TotalBetAppPath, "~/WagerAccount/GetByEvent"),
                  new Dictionary<string, object>
                  {
                      ["CustId"] = CustId,
                      ["CustName"] = CustName,
                      ["CustLevelId"] = CustLevel.Value,
                      ["MatchId"] = MatchId,
                      ["BetType"] = BetType,
                      ["LiveIndicator"] = LiveIndicator,
                      ["BetTeam"] = BetTeam,
                      ["Stake"] = Stake,
                      ["StakeCondition"] = StakeCondition
                  });
    }
}