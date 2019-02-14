namespace CN88.Features.Site.CustomerList
{
    using CN88.Features.Models;
    using CN88.Features.Site.Requests;
    using MediatR;

    public class CustomerStatusCommand : BaseQuery, IRequest<Result>
    {
        public CustomerStatusCommand(IUser viewer) : base(viewer)
        {
        }

        public byte UpdateStatusId { get; set; }
    }
}