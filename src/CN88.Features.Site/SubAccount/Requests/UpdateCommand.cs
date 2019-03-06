namespace CN88.Features.Site.SubAccount.Requests
{
    using Features.Models;
    using MediatR;
    using Site.Requests;

    public class UpdateCommand : BaseQuery, IRequest<Result>
    {
        public UpdateCommand(IUser viewer)
            : base(viewer)
        {
            LanguageKey = viewer.LanguageKey;
        }

        public int SubAccId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool Closed { get; set; }

        public string LanguageKey { get; }

        public override bool IsValid()
            => base.IsValid() && SubAccountId == 0;
    }
}