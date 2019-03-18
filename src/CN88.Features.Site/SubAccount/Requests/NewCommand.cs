namespace CN88.Features.Site.SubAccount.Requests
{
    using System.Collections.Generic;
    using Features.Models;
    using MediatR;
    using Site.Requests;

    public class NewCommand : BaseQuery, IRequest<Result>
    {
        public NewCommand(IUser viewer)
            : base(viewer)
        {
            LanguageKey = viewer.LanguageKey;
        }

        public string Suffix1 { get; set; }

        public string Suffix2 { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public string PageAccess { get; set; }

        public string LanguageKey { get; }

        public IEnumerable<string> InvalidPasswords
        {
            get
            {
                yield return ViewerName;
                yield return NickName;
            }
        }

        public override bool IsValid()
            => base.IsValid() && SubAccountId == 0 && !GetViewer().IsInternal;
    }
}