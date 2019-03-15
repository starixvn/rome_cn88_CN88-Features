namespace CN88.Features.Site.BetLists.Requests
{
    using System;
    using Features.Models;
    using MediatR;
    using Site.Requests;
    using ViewModels;

    public class EventWagerDetailsQuery : BaseQuery, IRequest<EventWagerDetailsViewModel>
    {
        public EventWagerDetailsQuery(IUser viewer) : base(viewer)
        {
            DisplayVNIP = viewer.IsAdminCanViewVNIP;
            ViewerIdOrViewerAdminId = viewer.IsInternal ? viewer.AdminId : viewer.Id;
            PageSize = 50;
            PageIndex = 1;
            LimitedDateToShowInfo = viewer.LimitedDateToShowInfo;
        }

        public bool DisplayVNIP { get; }

        public int ViewerIdOrViewerAdminId { get; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public DateTime LimitedDateToShowInfo { get; }

        public int StakeCondition { get; set; }

        public decimal Stake { get; set; }

        public int MatchId { get; set; }

        public int BetType { get; set; }

        public int LiveIndicator { get; set; }

        public string BetTeam { get; set; }
    }
}