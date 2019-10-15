namespace CN88.Features.Site.BetLists.Requests
{
    using System;
    using Features.Models;
    using MediatR;
    using ViewModels;

    public class BetListOfEBReportQuery : BetListBaseQuery, IRequest<BetListOfEBReportViewModel>
    {
        public BetListOfEBReportQuery(IUser viewer) : base(viewer)
        {
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string UserSelectedSportGroupId { get; set; }

        public int UserSelectedBetTypeId { get; set; }

        public int UserSelectedLiveStatusId { get; set; }

        public int Type { get; set; }

        public int DisplayMode { get; set; }

        public string ReportMainActionPath { get; set; }

        public bool ExcludeDataTest { get; set; }
    }
}