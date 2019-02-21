namespace CN88.Features.Site.Reports.Requests
{
    using System;
    using Features.Models;
    using Features.Shared.Utils;
    using MediatR;
    using Site.Requests;
    using ViewModels;

    public class ReportQuery : BaseQuery, IRequest<ReportViewModel>
    {
        public ReportQuery(IUser viewer)
            : base(viewer)
        {
            FromDate = SystemTime.Today;
            ToDate = SystemTime.Today;
        }

        public int ReportType { get; set; }

        public int UserSelectedSportGroupId { get; set; }

        public int UserSelectedBetTypeId { get; set; }

        public int UserSelectedLiveStatusId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public bool IsOwner => CustId == ViewerId;
    }
}