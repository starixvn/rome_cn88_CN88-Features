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

    public class BetListOfReportViewModel
    {
        public BetListOfReportViewModel(
            BetListOfReportQuery message,
            BetListData betListData,
            ITicketHelper ticketHelper,
            IBetListCN88HtmlBuilder cn88Builder,
            IBetListHTMLBuilder builder,
            string[] excludingTicketStatus = null)
        {
            CustLevel = message.CustLevel;
            DisplayVNIP = message.DisplayVNIP;

            NoOfColumns = CustLevel.IsSuperLevel() ? 11 : CustLevel.IsMasterLevel() ? 9 : 8;

            BetListData = betListData;
            BetListTotal = new BetListTotal(betListData, excludingTicketStatus);
            TicketHelper = ticketHelper;
            CN88Builder = cn88Builder;
            Builder = builder;

            FromDate = message.FromDate;
            ToDate = message.ToDate;
            SportTypeGroup = message.SportTypeGroup;
            BetTypeGroup = message.BetTypeGroup;
            CustId = message.CustId;
            Type = message.Type;

            ViewerIdOrViewerAdminId = message.ViewerIdOrViewerAdminId;
            ViewerLevelId = message.ViewerLevelId;
        }

        public UserLevel CustLevel { get; }

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

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int SportTypeGroup { get; }

        public int BetTypeGroup { get; }

        public int LiveIndicator { get; }

        public int Type { get; }

        public int CustId { get; }

        public int ViewerIdOrViewerAdminId { get; }

        public byte ViewerLevelId { get; }

        public decimal CompanyPositionTaking(TicketDto ticket)
        {
            return 100 - ticket.AgentPositionTaking - ticket.MasterPositionTaking - ticket.SuperPositionTaking;
        }
    }
}