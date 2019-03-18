namespace CN88.Features.Site.SubAccount.Requests
{
    using Features.Models;
    using MediatR;
    using Site.Requests;

    public class DeleteCommand : BaseQuery, IRequest<Result>
    {
        public DeleteCommand(IUser viewer)
            : base(viewer)
        {
            LanguageKey = viewer.LanguageKey;
        }

        public string LanguageKey { get; }

        public override bool IsValid()
            => base.IsValid() && SubAccountId == 0 && !GetViewer().IsInternal;
    }
}