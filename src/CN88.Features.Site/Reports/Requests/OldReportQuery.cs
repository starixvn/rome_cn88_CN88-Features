namespace CN88.Features.Site.Reports.Requests
{
    using Features.Models;
    using MediatR;
    using ViewModels;

    public class OldReportQuery : ReportBaseQuery, IRequest<OldReportViewModel>
    {
        public OldReportQuery(IUser viewer)
            : base(viewer)
        {
        }
    }
}