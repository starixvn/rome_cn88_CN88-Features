namespace CN88.Features.Site.BetLists.Requests
{
    using Features.Models;
    using MediatR;
    using ViewModels;

    public class OutrightWagerByAccountQuery : BetListBaseQuery, IRequest<OutrightWagerByAccountViewModel>
    {
        public OutrightWagerByAccountQuery(IUser viewer) : base(viewer)
        {
        }

        public int MatchId { get; set; }

        public int StakeCondition { get; set; }

        public decimal Stake { get; set; }
    }
}