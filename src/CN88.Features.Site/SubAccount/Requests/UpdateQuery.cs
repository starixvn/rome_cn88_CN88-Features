namespace CN88.Features.Site.SubAccount.Requests
{
    using CN88.Features.Site.SubAccount.ViewModels;
    using Features.Models;
    using MediatR;
    using Site.Requests;

    public class UpdateQuery : BaseQuery, IRequest<EditViewModel>
    {
        public UpdateQuery(IUser viewer)
            : base(viewer)
        {
        }

        public bool ShowAccountManagementPermission => ActorLevel.IsSuperLevel();

        public override bool IsValid()
            => base.IsValid() && CustId != ViewerId;
    }
}