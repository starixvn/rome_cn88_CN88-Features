namespace CN88.Features.Site.Reports.Requests
{
    using Features.Models;
    using MediatR;
    using ViewModels;

    public class IndexQuery : ReportBaseQuery, IRequest<IndexViewModel>
    {
        public IndexQuery(IUser viewer)
            : base(viewer)
        {
        }
    }
}