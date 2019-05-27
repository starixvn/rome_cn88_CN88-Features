namespace CN88.Features.Site.BetLists.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using BetList.Core.Builders;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Features.BetLists.Models;
    using Requests;
    using Shared.ViewModels;
    using Shared.ViewModels.Components;

    public class TotalBetParlayViewModel : BetListViewModelBase
    {
        public TotalBetParlayViewModel(
            TotalBetParlayQuery message,
            BetListData betListData,
            ITicketHelper ticketHelper,
            IBetListCN88HtmlBuilder cn88Builder,
            IBetListHTMLBuilder builder,
            IEnumerable<SelectListItemExt> sportGroups,
            IEnumerable<SelectListItemExt> stakeModes,
            IEnumerable<SelectListItemExt> viewModes,
            bool isMobile,
            string[] excludingTicketStatus = null)
        {
            Divisor = message.ActorLevel.IsHeadline() ? 100 : 1;

            CustLevel = message.ActorLevel;
            DisplayVNIP = message.DisplayVNIP;
            CustName = message.CustName;
            CustId = message.CustId;

            SportGroupId = message.SportGroupId;
            ViewMode = message.ViewMode;
            StakeMode = message.StakeMode;
            StakeLimit = message.StakeLimit;

            NoOfColumns = CalculateNumberOfColumns(10);

            BetListData = betListData;
            BetListTotal = new BetListTotal(betListData, Divisor, excludingTicketStatus);
            TicketHelper = ticketHelper;
            CN88Builder = cn88Builder;
            Builder = builder;

            ViewerIdOrViewerAdminId = message.ViewerIdOrViewerAdminId;
            ViewerLevelId = message.ViewerLevelId;

            PageIndex = message.PageIndex;
            PageSize = message.PageSize;

            SportGroupSelectionViewModel = new SingleSelectionViewModel(sportGroups, message.SportGroupId);
            StakeSelectionViewModel = new StakeSelectionViewModel(stakeModes, message.StakeMode, message.StakeLimit);
            ViewModeSelectionViewModel = new SingleSelectionViewModel(viewModes, message.ViewMode);
            PagerViewModel = new PagerViewModel(message.PageIndex, message.PageSize, betListData.Tickets.Count());

            TotalPayout = BetListData.Tickets.Sum(ticket => ticket.Payout) / Divisor;

            IsMobile = isMobile;
            IsShowPayout = true;
            IsShowWinLoss = false;

            BreadcrumbLevel = message.BreadcrumbLevel;
        }

        public int SportGroupId { get; }

        public int ViewMode { get; }

        public int StakeMode { get; }

        public decimal StakeLimit { get; }

        public SingleSelectionViewModel SportGroupSelectionViewModel { get; }

        public StakeSelectionViewModel StakeSelectionViewModel { get; }

        public SingleSelectionViewModel ViewModeSelectionViewModel { get; }
    }
}