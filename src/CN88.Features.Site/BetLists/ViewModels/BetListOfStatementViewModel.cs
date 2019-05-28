namespace CN88.Features.Site.BetLists.ViewModels
{
    using System;
    using BetList.Core.Builders;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Features.BetLists.Models;
    using Requests;

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
            : base(message, betListData, ticketHelper, cn88Builder, builder, isMobile, excludingTicketStatus)
        {
            Date = message.Date;
            SportTypeGroup = message.SportTypeGroup;
        }

        public DateTime Date { get; }

        protected override int GetMaxColumn() => 11;
    }
}