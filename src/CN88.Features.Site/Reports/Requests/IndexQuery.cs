namespace CN88.Features.Site.Reports.Requests
{
    using System;
    using Features.Shared.Utils;
    using MediatR;
    using ViewModels;

    public class IndexQuery : IRequest<IndexViewModel>
    {
        public IndexQuery()
        {
            FromDate = SystemTime.Today.AddDays(-1);
            ToDate = SystemTime.Today.AddDays(-1);
        }

        public int DisplayMode { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}