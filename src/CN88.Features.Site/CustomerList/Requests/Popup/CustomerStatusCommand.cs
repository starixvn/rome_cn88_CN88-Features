namespace CN88.Features.Site.CustomerList
{
    using Features.Models;
    using MediatR;

    public class CustomerStatusCommand : BaseCommand, IRequest<Result>
    {
        public CustomerStatusCommand(IUser viewer) : base(viewer)
        {
        }

        public byte OldStatusId { get; set; }

        public byte NewStatusId { get; set; }
    }
}