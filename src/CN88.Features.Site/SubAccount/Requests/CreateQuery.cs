namespace CN88.Features.Site.SubAccount.Requests
{
    using Features.Models;
    using MediatR;
    using Site.Requests;
    using ViewModels;

    public class CreateQuery : BaseQuery, IRequest<CreateViewModel>
    {
        public CreateQuery(IUser viewer)
            : base(viewer)
        {
        }

        public Result Result { get; set; }

        public override bool IsValid()
            => base.IsValid() && SubAccountId == 0;
    }
}