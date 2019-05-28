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
            : base(message, betListData, ticketHelper, cn88Builder, builder, isMobile, excludingTicketStatus)
        {
            SportGroupId = message.SportGroupId;
            ViewMode = message.ViewMode;
            StakeMode = message.StakeMode;
            StakeLimit = message.StakeLimit;

            NoOfColumns = CalculateNumberOfColumns(10);

            SportGroupSelectionViewModel = new SingleSelectionViewModel(sportGroups, message.SportGroupId);
            StakeSelectionViewModel = new StakeSelectionViewModel(stakeModes, message.StakeMode, message.StakeLimit);
            ViewModeSelectionViewModel = new SingleSelectionViewModel(viewModes, message.ViewMode);

            TotalPayout = BetListData.Tickets.Sum(ticket => ticket.Payout) / Divisor;

            IsShowPayout = true;
            IsShowWinLoss = false;
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