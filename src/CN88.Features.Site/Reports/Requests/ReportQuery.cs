namespace CN88.Features.Site.Reports.Requests
{
    using Features.Models;
    using MediatR;
    using ViewModels;

    public class ReportQuery : ReportBaseQuery, IRequest<ReportViewModel>
    {
        public ReportQuery(IUser viewer)
            : base(viewer)
        {
        }
    }
}