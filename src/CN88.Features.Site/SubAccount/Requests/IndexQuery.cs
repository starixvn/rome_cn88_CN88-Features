namespace CN88.Features.Site.SubAccount.Requests
{
    using Features.Models;
    using MediatR;
    using Site.Requests;
    using ViewModels;

    public class IndexQuery : BaseQuery, IRequest<IndexViewModel>
    {
        public IndexQuery(IUser viewer)
            : base(viewer)
        {
        }

        public string UserCode { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}