namespace CN88.Features.Site.CustomerList.Requests
{
    using Features.Models;
    using MediatR;
    using Site.Requests;
    using ViewModels;

    public class OnlineMembersQuery : BaseQuery, IRequest<OnlineMemberViewModel>
    {
        public OnlineMembersQuery(IUser viewer) : base(viewer)
        {
            PageIndex = 1;
            PageSize = 50;
        }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}