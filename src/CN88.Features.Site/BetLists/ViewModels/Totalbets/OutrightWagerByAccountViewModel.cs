namespace CN88.Features.Site.BetLists.ViewModels
{
    using System.Collections.Generic;
    using Alpha.Web.Helpers;
    using BetList.Core.Builders;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Features.BetLists.Models;
    using Features.Shared.Configurations;
    using Requests;
    using Shared.Helpers;

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
            : base(message, betListData, ticketHelper, cn88Builder, builder, isMobile, excludingTicketStatus)
        {
            this.message = message;

            Stake = message.Stake;
            StakeCondition = message.StakeCondition;
            MatchId = message.MatchId;
            NoOfColumns = CalculateNumberOfColumns();

            var reportPath = AppPathHelper.GetAppPath(AppSettings.TotalBetAppPath, "~/WagerAccount/GetByOutright");
            BreadcrumbParamByLevel = CalculateBreadcrumbParamByLevel(reportPath);

            IsShowWinLoss = false;
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