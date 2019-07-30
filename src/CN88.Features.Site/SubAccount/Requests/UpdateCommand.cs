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
        }

        [AllowHtml]
        public string Name { get; set; }

        public string PageAccess { get; set; }

        public bool IsSelectAllDownline { get; set; }

        public string DownlineId { get; set; }

        public override bool IsValid()
            => base.IsValid()
            && !IsInternal
            && !IsSelectAllDownline
            && !string.IsNullOrWhiteSpace(DownlineId);
    }
}
