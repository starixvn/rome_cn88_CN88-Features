namespace CN88.Features.Site.SubAccount.Requests
{
    using CN88.Features.Models;
    using CN88.Features.Site.Requests;
    using MediatR;

    public class DeleteCommand : BaseQuery, IRequest<Result>
    {
        public DeleteCommand(IUser viewer)
            : base(viewer)
        {
        }

        public override bool IsValid()
            => base.IsValid() && !IsInternal;
    }
}