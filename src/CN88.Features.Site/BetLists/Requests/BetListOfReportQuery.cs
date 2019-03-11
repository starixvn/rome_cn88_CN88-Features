namespace CN88.Features.Site.BetLists.Requests
{
    using System;
    using Features.Models;
    using MediatR;
    using Site.Requests;
    using ViewModels;

    public class BetListOfReportQuery : BaseQuery, IRequest<BetListOfReportViewModel>
    {
        public BetListOfReportQuery(IUser viewer) : base(viewer)
        {
            DisplayVNIP = viewer.IsAdminCanViewVNIP;
            ViewerIdOrViewerAdminId = viewer.IsInternal ? viewer.AdminId : viewer.Id;
            PageSize = 50;
            PageIndex = 1;
            LimitedDateToShowInfo = viewer.LimitedDateToShowInfo;
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int SportTypeGroup { get; set; }

        public int BetTypeGroup { get; set; }

        public int LiveIndicator { get; set; }

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