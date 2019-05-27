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

    public class OutrightWagerDetailsViewModel : BetListViewModelBase
    {
        public OutrightWagerDetailsViewModel(
              OutrightWagerDetailsQuery message,
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

            NoOfColumns = CalculateNumberOfColumns();

            BetListData = betListData;
            BetListTotal = new BetListTotal(betListData, Divisor, excludingTicketStatus);

            MatchId = message.MatchId;
            PagerViewModel = new PagerViewModel(message.PageIndex, message.PageSize, betListData.Tickets.Count());

            IsMobile = isMobile;

            IsShowWinLoss = false;
            BreadcrumbLevel = message.BreadcrumbLevel;
        }

        public int MatchId { get; }

        public decimal Stake { get; }

        public int StakeCondition { get; }

        public string WagerByAccountLink()
            => UrlHelper.ReplaceOrAddQueryStringParamsFunc(
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