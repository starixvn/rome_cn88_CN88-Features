namespace CN88.Features.Site.CustomerList
{
    using CN88.Features.Models;
    using CN88.Features.Site.Requests;
    using MediatR;

    public class CreditCommand : BaseQuery, IRequest<Result>
    {
        private readonly IUser viewer;

        public CreditCommand(IUser viewer) : base(viewer)
        {
            this.viewer = viewer;
        }

        public decimal CreditAmount { get; set; }
    }
}