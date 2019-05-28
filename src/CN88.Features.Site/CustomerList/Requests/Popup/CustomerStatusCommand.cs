namespace CN88.Features.Site.CustomerList
{
    using Features.Models;
    using MediatR;
    using Site.Requests;

    public class CustomerStatusCommand : BaseQuery, IRequest<Result>
    {
        public CustomerStatusCommand(IUser viewer)
            : base(viewer)
        {
        }

        public byte OldStatusId { get; set; }

        public byte NewStatusId { get; set; }

        public override bool IsValid() => base.IsValid() && !IsInternal;
    }
}