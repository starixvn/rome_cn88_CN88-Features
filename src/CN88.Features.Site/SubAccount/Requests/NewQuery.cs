namespace CN88.Features.Site.SubAccount.Requests
{
    using Features.Models;
    using MediatR;
    using Site.Requests;
    using ViewModels;

    public class NewQuery : BaseQuery, IRequest<NewViewModel>
    {
        public NewQuery(IUser viewer)
            : base(viewer)
        {
        }

        public bool ShowAccountManagementPermission => ActorLevel.IsSuperLevel();
    }
}