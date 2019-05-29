namespace CN88.Features.Site.CustomerList
{
    using Features.Models;
    using MediatR;

    public class CreditCommand : BaseCommand, IRequest<Result>
    {
        public CreditCommand(IUser viewer) : base(viewer)
        {
        }

        public decimal CreditAmount { get; set; }
    }
}