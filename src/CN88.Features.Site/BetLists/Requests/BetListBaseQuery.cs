namespace CN88.Features.Site.BetLists.Requests
{
    using System;
    using CN88.Features.Models;
    using CN88.Features.Shared.Constants;
    using CN88.Features.Site.Requests;

    public class BetListBaseQuery : BaseQuery
    {
        protected BetListBaseQuery(IUser viewer) : base(viewer)
        {
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
    }
}