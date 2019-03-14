namespace CN88.Features.Site.BetLists.Requests
{
    using System;
    using Features.Models;
    using MediatR;
    using Site.Requests;
    using ViewModels;

    public class BetListByAccountQuery : BaseQuery, IRequest<BetListByAccountViewModel>
    {
        public BetListByAccountQuery(IUser viewer) : base(viewer)
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

        public int MatchId { get; }

        public int BetTypeGroup { get; }

        public int LiveIndicator { get; }

        public string BetTeam { get; }

        public int StakeCondition { get; }

        public decimal Stake { get; }
    }
}