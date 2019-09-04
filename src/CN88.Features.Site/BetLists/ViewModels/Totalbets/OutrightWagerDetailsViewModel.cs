namespace CN88.Features.Site.BetLists.ViewModels
{
    using System.Collections.Generic;
    using BetList.Core.Builders;
    using CN88.Web.Helpers;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Features.BetLists.Models;
    using Features.Shared.Configurations;
    using Requests;
    using Shared.Helpers;

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
            : base(message, betListData, ticketHelper, cn88Builder, builder, isMobile, excludingTicketStatus)
        {
            Stake = message.Stake;
            StakeCondition = message.StakeCondition;
            MatchId = message.MatchId;

            IsShowWinLoss = false;
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
