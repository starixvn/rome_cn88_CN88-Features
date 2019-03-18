namespace CN88.Features.Site.SubAccount.Requests
{
    using System.Collections.Generic;
    using Features.Models;
    using MediatR;
    using Site.Requests;

    public class CreateCommand : BaseQuery, IRequest<Result>
    {
        public CreateCommand(IUser viewer)
            : base(viewer)
        {
            LanguageKey = viewer.LanguageKey;
        }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PageAccess { get; set; }

        public string LanguageKey { get; }

        public IEnumerable<string> InvalidPasswords
        {
            get
            {
                yield return ViewerName;
                yield return NickName;
                yield return UserName;
            }
        }

        public override bool IsValid()
            => base.IsValid() && SubAccountId == 0 && !GetViewer().IsInternal;
    }
}