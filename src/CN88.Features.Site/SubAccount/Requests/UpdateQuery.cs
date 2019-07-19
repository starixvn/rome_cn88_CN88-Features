namespace CN88.Features.Site.SubAccount.Requests
{
    using Features.Models;
    using Features.SubAccount.Models;
    using MediatR;
    using Site.Requests;

    public class UpdateQuery : BaseQuery, IRequest<SubAccountItem>
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