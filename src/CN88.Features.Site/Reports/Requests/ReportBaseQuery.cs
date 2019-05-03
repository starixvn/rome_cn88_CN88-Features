namespace CN88.Features.Site.Reports.Requests
{
    using System;
    using Features.Models;
    using Features.Reports.Constants;
    using Features.Shared.Constants;
    using Features.Shared.Utils;
    using Site.Requests;

    public class ReportBaseQuery : BaseQuery
    {
        private bool? isSettlement;

        public ReportBaseQuery(IUser viewer) : base(viewer)
        {
            CustId = viewer.Id;
            CustName = viewer.Name;
            CustLevelId = viewer.LevelId;

            FromDate = SystemTime.Today;
            ToDate = SystemTime.Today;

            UserSelectedLiveStatusId = LiveStatusConstants.AllMarkets;

            PageSize = PagerConstant.DefaultPageSize;
            PageIndex = PagerConstant.DefaultPageIndex;
        }

        public int DisplayMode { get; set; }

        public int ReportType { get; set; }

        public int UserSelectedSportGroupId { get; set; }

        public int UserSelectedBetTypeId { get; set; }

        public int UserSelectedLiveStatusId { get; set; }

        public int UserSelectedPeriodId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string SortPropertyName { get; set; }

        public bool IsDescendingSort { get; set; }

        public bool? IsSettlement
        {
            get
            {
                return isSettlement == null
                    ? ReportType == ReportTypeConstants.SettledWager
                    : isSettlement.Value;
            }
            set
            {
                isSettlement = value;
            }
        }
    }
}