namespace CN88.Features.Site.Reports.Requests
{
    using System;
    using Features.Models;
    using MediatR;
    using Site.Requests;
    using ViewModels;

    public class PTHistoryQuery : BaseQuery, IRequest<PTHistoryViewModel>
    {
        public PTHistoryQuery(IUser viewer)
            : base(viewer)
        {
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}