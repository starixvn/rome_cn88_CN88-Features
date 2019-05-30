namespace CN88.Features.Site.CustomerList
{
    using Features.Models;
    using MediatR;

    public class UnlockCustomerCommand : BaseCommand, IRequest<Result>
    {
        public UnlockCustomerCommand(IUser viewer) : base(viewer)
        {
        }
    }
}