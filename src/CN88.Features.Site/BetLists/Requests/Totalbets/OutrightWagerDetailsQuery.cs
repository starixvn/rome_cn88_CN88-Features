namespace CN88.Features.Site.BetLists.Requests
{
    using Features.Models;
    using MediatR;
    using ViewModels;

    public class OutrightWagerDetailsQuery : BetListBaseQuery, IRequest<OutrightWagerDetailsViewModel>
    {
        public OutrightWagerDetailsQuery(IUser viewer) : base(viewer)
        {
        }

        public int StakeCondition { get; set; }

        public decimal Stake { get; set; }

        public int MatchId { get; set; }
    }
}