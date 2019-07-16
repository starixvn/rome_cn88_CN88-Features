namespace CN88.Features.Site.SubAccount.Requests
{
    using CN88.Features.CustomerList;
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

        public bool ShowActionButtons => ViewerStatusId == CustomerStatus.Active;
    }
}