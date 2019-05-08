namespace CN88.Features.Site.BetLists.Requests.WagerMonitoring
{
    using Features.Models;
    using MediatR;
    using Site.Requests;
    using ViewModels.WagerMonitoring;

    public class EventQuery : BaseQuery, IRequest<MultipleSelectViewModel>
    {
        public EventQuery(IUser viewer) : base(viewer)
        {
        }

        public int SportGroup { get; set; }

        public string MatchIds { get; set; }
    }
}