namespace CN88.Features.Site.Reports.Requests
{
    using Features.Models;
    using MediatR;
    using ViewModels;

    public class OldIndexQuery : ReportBaseQuery, IRequest<OldIndexViewModel>
    {
        public OldIndexQuery(IUser viewer)
            : base(viewer)
        {
        }
    }
}