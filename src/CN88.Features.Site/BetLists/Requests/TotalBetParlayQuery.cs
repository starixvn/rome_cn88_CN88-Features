namespace CN88.Features.Site.BetLists.Requests
{
    using System;
    using Features.Models;
    using Features.Shared.Constants;
    using MediatR;
    using Site.Requests;
    using ViewModels;

    public class TotalBetParlayQuery : BaseQuery, IRequest<TotalBetParlayViewModel>
    {
        public TotalBetParlayQuery(IUser viewer) : base(viewer)
        {
            CustId = viewer.Id;
            CustName = viewer.Name;
            CustLevelId = viewer.LevelId;

            DisplayVNIP = viewer.IsAdminCanViewVNIP;
            ViewerIdOrViewerAdminId = viewer.IsInternal ? viewer.AdminId : viewer.Id;
            PageSize = PagerConstant.DefaultPageSize;
            PageIndex = PagerConstant.DefaultPageIndex;
            LimitedDateToShowInfo = viewer.LimitedDateToShowInfo;
        }

        public bool DisplayVNIP { get; }

        public int ViewerIdOrViewerAdminId { get; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public DateTime LimitedDateToShowInfo { get; }

        public int SportGroupId { get; set; }

        public int ViewMode { get; set; }

        public int StakeMode { get; set; } = 1;

        public decimal StakeLimit { get; set; }
    }
}