namespace CN88.Features.Site.SubAccount.Requests
{
    using Features.Models;
    using MediatR;
    using Site.Requests;

    public class UpdatePermissionCommand : BaseQuery, IRequest<Result>
    {
        public UpdatePermissionCommand(IUser viewer)
            : base(viewer)
        {
            LanguageKey = viewer.LanguageKey;
        }

        public int SubAccId { get; set; }

        public string PageAccess { get; set; }

        public string LanguageKey { get; }

        public override bool IsValid()
            => base.IsValid() && SubAccountId == 0;
    }
}