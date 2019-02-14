namespace CN88.Features.Site.Reports.Requests
{
    using System;
    using MediatR;
    using ViewModels;

    public class MainQuery : IRequest<MainViewModel>
    {
        public MainQuery()
        {
            FromDate = DateTime.Now.Date;
            ToDate = DateTime.Now.Date;
        }

        public int ReportType { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}