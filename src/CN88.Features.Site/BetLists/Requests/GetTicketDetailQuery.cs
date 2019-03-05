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

        public string RefNo { get; set; }

        public string TransId { get; set; }

        public string BetId { get; set; }
    }
}