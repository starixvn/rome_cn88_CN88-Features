namespace CN88.Features.Site.CustomerList
{
    using Features.Models;
    using MediatR;
    using Site.Requests;

    public class UnlockCommand : BaseCommand, IRequest<Result>
    {
        public UnlockCommand(IUser viewer) : base(viewer)
        {
        }
    }
}