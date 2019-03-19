namespace CN88.Features.Site.BetLists.Requests
{
    using System;
    using Features.Models;
    using Features.Shared.Constants;
    using MediatR;
    using Site.Requests;
    using ViewModels;

    public class BetListOfReportQuery : BaseQuery, IRequest<BetListOfReportViewModel>
    {
        public BetListOfReportQuery(IUser viewer) : base(viewer)
        {
            DisplayVNIP = viewer.IsAdminCanViewVNIP;
            ViewerIdOrViewerAdminId = viewer.IsInternal ? viewer.AdminId : viewer.Id;
            PageSize = PagerConstant.DefaultPageSize;
            PageIndex = PagerConstant.DefaultPageIndex;
            LimitedDateToShowInfo = viewer.LimitedDateToShowInfo;
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int UserSelectedSportGroupId { get; set; }

        public int UserSelectedBetTypeId { get; set; }

        public int UserSelectedLiveStatusId { get; set; }

        public int Type { get; set; }

        public bool DisplayVNIP { get; }

        public int ViewerIdOrViewerAdminId { get; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int DisplayMode { get; set; }

        public string ReportMainActionPath { get; set; }

        public bool IsOldReport { get; set; }

        public DateTime LimitedDateToShowInfo { get; }
    }
}