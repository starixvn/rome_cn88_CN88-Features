namespace CN88.Features.Site.BetLists.Requests.WagerMonitoring
{
    using Features.Models;
    using MediatR;
    using ViewModels.WagerMonitoring;

    public class WagerMonitoringFilterQuery : WagerMonitoringBaseQuery, IRequest<WagerMonitoringFilterViewModel>
    {
        public WagerMonitoringFilterQuery(IUser viewer) : base(viewer)
        {
        }
    }
}