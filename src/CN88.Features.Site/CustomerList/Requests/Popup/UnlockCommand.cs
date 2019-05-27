namespace CN88.Features.Site.CustomerList
{
    using Features.Models;
    using MediatR;
    using Site.Requests;

    public class UnlockCommand : BaseQuery, IRequest<Result>
    {
        public UnlockCommand(IUser viewer)
            : base(viewer)
        {
            LanguageKey = viewer.LanguageKey;
        }

        public string LanguageKey { get; }

        public override bool IsValid() => base.IsValid() && !IsInternal;
    }
}