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

        public int SportGroupId { get; set; }

        public int BetTypeId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public bool IsOwner => CustId == ViewerId;
    }
}