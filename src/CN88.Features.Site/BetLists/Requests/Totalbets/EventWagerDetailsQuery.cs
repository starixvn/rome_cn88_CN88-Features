namespace CN88.Features.Site.BetLists.Requests
{
    using Features.Models;
    using MediatR;
    using ViewModels;

    public class EventWagerDetailsQuery : BetListBaseQuery, IRequest<EventWagerDetailsViewModel>
    {
        public EventWagerDetailsQuery(IUser viewer) : base(viewer)
        {
        }

        public int StakeCondition { get; set; }

        public decimal Stake { get; set; }

        public int MatchId { get; set; }

        public int BetType { get; set; }

        public int LiveIndicator { get; set; }

        public string BetTeam { get; set; }
    }
}