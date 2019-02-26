namespace CN88.Features.Site.SubAccount.Requests
{
    using System.ComponentModel.DataAnnotations;
    using Features.Models;
    using Features.SubAccount.AppResources;
    using MediatR;
    using Site.Requests;

    public class CreateCommand : BaseQuery, IRequest<Result>
    {
        public CreateCommand(IUser viewer)
            : base(viewer)
        {
        }

        [Required(ErrorMessageResourceName = "RequiredFieldError", ErrorMessageResourceType = typeof(SubAccountResource))]
        public string UserCode { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldError", ErrorMessageResourceType = typeof(SubAccountResource))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldError", ErrorMessageResourceType = typeof(SubAccountResource))]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldError", ErrorMessageResourceType = typeof(SubAccountResource))]
        public string LoginId { get; set; }

        public string PageAccess { get; set; }
    }
}