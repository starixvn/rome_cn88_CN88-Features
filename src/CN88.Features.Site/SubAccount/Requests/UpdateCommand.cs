namespace CN88.Features.Site.SubAccount.Requests
{
    using System.Web.Mvc;
    using Features.Models;
    using MediatR;
    using Site.Requests;

    public class UpdateCommand : BaseQuery, IRequest<Result>
    {
        public UpdateCommand(IUser viewer)
            : base(viewer)
        {
            LanguageKey = viewer.LanguageKey;
            IsInternal = viewer.IsInternal;
        }

        [AllowHtml]
        public string Name { get; set; }

        public string PageAccess { get; set; }

        public string LanguageKey { get; }

        public bool IsInternal { get; }

        public override bool IsValid()
            => base.IsValid() && SubAccountId == 0 && !IsInternal;
    }
}