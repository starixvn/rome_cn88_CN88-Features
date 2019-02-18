namespace CN88.Features.Site.Reports.Requests
{
    using System;
    using Features.Models;
    using Site.Requests;

    public class ReportQuery : BaseQuery
    {
        public ReportQuery(IUser viewer)
            : base(viewer)
        {
        }

        public int ReportType { get; }

        public int SportGroupId { get; set; }

        public int BetTypeId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}