namespace CN88.Features.Site.BetLists.Requests
{
    using Features.Models;
    using MediatR;
    using Site.Requests;
    using ViewModels;

    public class GetParlayDetailQuery : BaseQuery, IRequest<ParlayDetailViewModel>
    {
        public GetParlayDetailQuery(IUser viewer) : base(viewer)
        {
        }

        public string RefNo { get; set; }

        public string TransId { get; set; }

        public string BetId { get; set; }
    }
}