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
    using Features.Shared.Models;
    using Requests;
    using Shared.Helpers;
    using Shared.ViewModels.Components;

    public class WagerDetailsViewModel
    {
        public WagerDetailsViewModel(
              WagerDetailsQuery message,
              BetListData betListData,
              ITicketHelper ticketHelper,
              IBetListCN88HtmlBuilder cn88Builder,
              IBetListHTMLBuilder builder,
              string[] excludingTicketStatus = null)
        {
            Divisor = (message.ActorLevel.IsHeadline()) ? 100 : 1;

            CustLevel = message.ActorLevel;
            DisplayVNIP = message.DisplayVNIP;
            CustName = message.CustName;
            CustId = message.CustId;
            Stake = message.CustId;
            StakeCondition = message.StakeCondition;

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
            MatchId = message.MatchId;
            PagerViewModel = new PagerViewModel(message.PageIndex, message.PageSize, betListData.Tickets.Count());
        }

        public UserLevel CustLevel { get; }

        public string CustName { get; }

        public int NoOfColumns { get; }

        public BetListData BetListData { get; }

        public BetListTotal BetListTotal { get; }

        public ITicketHelper TicketHelper { get; }

        public IBetListHTMLBuilder Builder { get; }

        public IBetListCN88HtmlBuilder CN88Builder { get; }

        public bool DisplayVNIP { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public string OldMainPath => MicroSites.OldMainUrl("~");

        public string ResultsPath => AppPathHelper.GetAppPath(AppSettings.ResultAppPath, "~");

        public string TotalBetsForecastPath => AppPathHelper.GetAppPath(AppSettings.TotalBetsForecastAppPath, "~");

        public int CustId { get; }

        public int ViewerIdOrViewerAdminId { get; }

        public byte ViewerLevelId { get; }

        public int Divisor { get; }

        public PagerViewModel PagerViewModel { get; private set; }

        public int MatchId { get; }

        public decimal Stake { get; }

        public int StakeCondition { get; }

        public string WagerByAccountLink()
        {
            return UrlHelper.ReplaceOrAddQueryStringParamsFunc(
                  AppPathHelper.GetAppPath(AppSettings.TotalBetAppPath, "~/WagerAccount/Index"),
                  new Dictionary<string, object>()
                  {
                      ["CustId"] = CustId,
                      ["CustLevelId"] = CustLevel.Value,
                      ["MatchId"] = MatchId,
                      ["Stake"] = Stake,
                      ["StakeCondition"] = StakeCondition
                  });
        }
    }
}