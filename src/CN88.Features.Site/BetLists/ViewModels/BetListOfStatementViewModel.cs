namespace CN88.Features.Site.BetLists.ViewModels
{
    using System;
    using Alpha.Web.Helpers;
    using BetList.Core.Builders;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Features.BetLists.Models;
    using Features.Shared.Configurations;
    using Features.Shared.Models;
    using Requests;

    public class BetListOfStatementViewModel
    {
        public BetListOfStatementViewModel(
            BetListOfStatementQuery message,
            BetListData betListData,
            ITicketHelper ticketHelper,
            IBetListCN88HtmlBuilder cn88Builder,
            IBetListHTMLBuilder builder,
            string[] excludingTicketStatus = null)
        {
            Divisor = (message.ActorLevel.IsHeadline()) ? 100 : 1;

            CustLevel = message.CustLevel;
            DisplayVNIP = message.DisplayVNIP;
            CustName = message.CustName;

            NoOfColumns = CustLevel.IsSuperLevel() || CustLevel.IsHeadline() ? 11 : CustLevel.IsMasterLevel() ? 9 : 8;

            BetListData = betListData;
            BetListTotal = new BetListTotal(betListData, Divisor, excludingTicketStatus);
            TicketHelper = ticketHelper;
            CN88Builder = cn88Builder;
            Builder = builder;

            Date = message.Date;
            SportTypeGroup = message.SportTypeGroup;
            CustId = message.CustId;

            ViewerIdOrViewerAdminId = message.ViewerIdOrViewerAdminId;
            ViewerLevelId = message.ViewerLevelId;
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

        public string OldMainPath => MicroSites.OldMainUrl("~");

        public string ResultsPath => AppPathHelper.GetAppPath(AppSettings.ResultAppPath, "~");

        public string TotalBetsForecastPath => AppPathHelper.GetAppPath(AppSettings.TotalBetsForecastAppPath, "~");

        public DateTime Date { get; }

        public int SportTypeGroup { get; }

        public int CustId { get; }

        public int ViewerIdOrViewerAdminId { get; }

        public byte ViewerLevelId { get; }

        public int Divisor { get; }

        public decimal CompanyPositionTaking(TicketDto ticket)
        {
            return 1 - ticket.AgentPositionTaking - ticket.MasterPositionTaking - ticket.SuperPositionTaking;
        }
    }
}