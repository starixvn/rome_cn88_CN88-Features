namespace CN88.Features.Site.BetLists.Requests
{
    using Features.BetLists.Models;
    using Features.Models;
    using MediatR;
    using Site.Requests;

    public class GetTicketDetailQuery : BaseQuery, IRequest<TicketDetailModel>
    {
        public GetTicketDetailQuery(IUser viewer) : base(viewer)
        {
        }

        public long RefNo { get; }

        public long TransId { get; }

        public long BetId { get; }
    }
}