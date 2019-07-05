namespace CN88.Features.Site.SubAccount.Requests
{
    using Features.Models;
    using MediatR;
    using Site.Requests;

    public class UnlockCommand : BaseQuery, IRequest<Result>
    {
        public UnlockCommand(IUser viewer)
            : base(viewer)
        {
        }

        public override bool IsValid()
            => base.IsValid() && !IsInternal;
    }
}