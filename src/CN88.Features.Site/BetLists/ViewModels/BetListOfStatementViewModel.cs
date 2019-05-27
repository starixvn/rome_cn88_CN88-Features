namespace CN88.Features.Site.BetLists.ViewModels
{
    using System;
    using System.Linq;
    using BetList.Core.Builders;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Features.BetLists.Models;
    using Requests;
    using Shared.ViewModels.Components;

    public class BetListOfStatementViewModel : BetListViewModelBase
    {
        public BetListOfStatementViewModel(
            BetListOfStatementQuery message,
            BetListData betListData,
            ITicketHelper ticketHelper,
            IBetListCN88HtmlBuilder cn88Builder,
            IBetListHTMLBuilder builder,
            bool isMobile,
            string[] excludingTicketStatus = null)
            : base(message, ticketHelper, cn88Builder, builder)
        {
            NoOfColumns = CalculateNumberOfColumns(11);

            BetListData = betListData;
            BetListTotal = new BetListTotal(betListData, Divisor, excludingTicketStatus);

            Date = message.Date;
            SportTypeGroup = message.SportTypeGroup;

            PagerViewModel = new PagerViewModel(message.PageIndex, message.PageSize, betListData.Tickets.Count());

            IsMobile = isMobile;
        }

        public DateTime Date { get; }
    }
}