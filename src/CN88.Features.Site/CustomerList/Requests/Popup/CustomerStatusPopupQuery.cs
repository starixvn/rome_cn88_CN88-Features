namespace CN88.Features.Site.CustomerList
{
    using CN88.Features.Models;
    using CN88.Features.Site.CustomerList.ViewModels;
    using CN88.Features.Site.Requests;
    using MediatR;

    public class CustomerStatusPopupQuery : BaseQuery, IRequest<StatusPopupViewModel>
    {
        public CustomerStatusPopupQuery(IUser viewer) : base(viewer)
        {
        }

        public int StatusId { get; set; }
    }
}