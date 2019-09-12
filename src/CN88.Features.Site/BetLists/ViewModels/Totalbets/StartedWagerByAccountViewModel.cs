namespace CN88.Features.Site.BetLists.ViewModels
{
    using System.Collections.Generic;
    using BetList.Core.Builders;
    using CN88.Web.Helpers;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Features.BetLists.Constants;
    using Features.BetLists.Models;
    using Features.Shared.Configurations;
    using Requests;
    using Shared.Helpers;

    public class StartedWagerByAccountViewModel : BetListViewModelBase
    {
        protected readonly StartedWagerByAccountQuery message;

        public StartedWagerByAccountViewModel(
              StartedWagerByAccountQuery message,
              BetListData betListData,
              ITicketHelper ticketHelper,
              IBetListCN88HtmlBuilder cn88Builder,
              IBetListHTMLBuilder builder,
              bool isMobile,
              string[] excludingTicketStatus = null)
            : base(message, betListData, ticketHelper, cn88Builder, builder, isMobile, excludingTicketStatus)
        {
            this.message = message;

            Stake = message.Stake;
            StakeCondition = message.StakeCondition;
            MatchId = message.MatchId;
            BetType = message.BetType;
            LiveIndicator = message.LiveIndicator;
            BetTeam = message.BetTeam;

            var reportPath = AppPathHelper.GetAppPath(AppSettings.TotalBetAppPath, "~/WagerAccount/GetByEvent");
            BreadcrumbParamByLevel = CalculateBreadcrumbParamByLevel(reportPath);

            IsShowWinLoss = false;
        }

        public decimal Stake { get; }

        public int StakeCondition { get; }

        public int MatchId { get; }

        public int BetType { get; }

        public int LiveIndicator { get; }

        public string BetTeam { get; }

        public string WagerDetailsLink()
        {
            return UrlHelper.ReplaceOrAddQueryStringParamsFunc(
                  AppPathHelper.GetAppPath(AppSettings.FeaturesAppPath, "~/BetLists/StartedWagerDetails"),
                  new Dictionary<string, object>
                  {
                      ["CustId"] = message.ViewerId,
                      ["CustName"] = message.ViewerName,
                      ["CustLevelId"] = message.ActorLevel.Value,
                      ["MatchId"] = MatchId,
                      ["BetType"] = BetType,
                      ["LiveIndicator"] = LiveIndicator,
                      ["BetTeam"] = BetTeam,
                      ["Stake"] = Stake,
                      ["StakeCondition"] = StakeCondition
                  });
        }

        public override string BuildBreadcrumbUrl()
            => UrlHelper.AddQueryStringParamsFunc(
                AppPathHelper.GetAppPath(AppSettings.TotalBetAppPath, "~/WagerAccount/GetByEvent"),
                new Dictionary<string, object>
                {
                    ["CustId"] = CustId,
                    ["CustName"] = CustName,
                    ["CustLevelId"] = CustLevel.Value,
                    ["MatchId"] = MatchId,
                    ["BetTypeGroup"] = BetType,
                    ["LiveIndicator"] = LiveIndicator,
                    ["BetTeam"] = BetTeam,
                    ["Stake"] = Stake,
                    ["StakeCondition"] = StakeCondition,
                    ["Type"] = TotalBetTypeConstants.Started
                });
    }
}
