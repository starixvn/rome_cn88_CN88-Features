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
        }
    }
}