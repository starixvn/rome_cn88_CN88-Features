namespace CN88.Features.Site.Reports.Requests
{
    using System;
    using Features.Models;
    using Features.Shared.Utils;
    using MediatR;
    using Site.Requests;
    using ViewModels;

    public class IndexQuery : BaseQuery, IRequest<IndexViewModel>
    {
        public IndexQuery(IUser viewer)
            : base(viewer)
        {
            FromDate = SystemTime.Today.AddDays(-1);
            ToDate = SystemTime.Today.AddDays(-1);
        }

        public int DisplayMode { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}