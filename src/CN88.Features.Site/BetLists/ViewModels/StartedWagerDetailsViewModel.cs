namespace CN88.Features.Site.BetLists.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Web.Helpers;
    using BetList.Core.Builders;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Features.BetLists.Constants;
    using Features.BetLists.Models;
    using Features.Shared.Configurations;
    using Features.Shared.Models;
    using Requests;
    using Shared.Helpers;
    using Shared.ViewModels.Components;

    public class StartedWagerDetailsViewModel : BetListTableViewModel
    {
        public StartedWagerDetailsViewModel(
              StartedWagerDetailsQuery message,
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
            BetType = message.BetType;
            LiveIndicator = message.LiveIndicator;
            BetTeam = message.BetTeam;

            NoOfColumns = CustLevel.IsSuperLevel() || CustLevel.IsHeadline() ? 9 : CustLevel.IsMasterLevel() ? 7 : 6;

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

            IsShowWinLoss = false;
            IsMobile = isMobile;
        }

        public int PageIndex { get; }

        public int PageSize { get; }

        public string OldMainPath => MicroSites.OldMainUrl("~");

        public string ResultsPath => AppPathHelper.GetAppPath(AppSettings.ResultAppPath, "~");

        public string TotalBetsForecastPath => AppPathHelper.GetAppPath(AppSettings.TotalBetsForecastAppPath, "~");

        public int CustId { get; }

        public int ViewerIdOrViewerAdminId { get; }

        public byte ViewerLevelId { get; }

        public PagerViewModel PagerViewModel { get; private set; }

        public int MatchId { get; }

        public int BetType { get; }

        public int LiveIndicator { get; }

        public string BetTeam { get; }

        public decimal Stake { get; }

        public int StakeCondition { get; }

        public string WagerByAccountLink()
        {
            return UrlHelper.ReplaceOrAddQueryStringParamsFunc(
                  AppPathHelper.GetAppPath(AppSettings.TotalBetAppPath, "~/WagerAccount/GetByEvent"),
                  new Dictionary<string, object>()
                  {
                      ["CustId"] = CustId,
                      ["CustName"] = CustName,
                      ["CustLevelId"] = CustLevel.Value,
                      ["MatchId"] = MatchId,
                      ["BetType"] = BetType,
                      ["LiveIndicator"] = LiveIndicator,
                      ["BetTeam"] = BetTeam,
                      ["Stake"] = Stake,
                      ["StakeCondition"] = StakeCondition,
                      ["Type"] = TotalBetTypeConstants.Started
                  });
        }
    }
}