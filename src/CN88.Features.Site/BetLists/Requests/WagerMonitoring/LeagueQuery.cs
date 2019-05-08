namespace CN88.Features.Site.BetLists.Requests.WagerMonitoring
{
    using Features.Models;
    using MediatR;
    using Site.Requests;
    using ViewModels.WagerMonitoring;

    public class LeagueQuery : BaseQuery, IRequest<MultipleSelectViewModel>
    {
        public LeagueQuery(IUser viewer) : base(viewer)
        {
        }

        public int SportGroup { get; set; }

        public string LeagueIds { get; set; }
    }
}