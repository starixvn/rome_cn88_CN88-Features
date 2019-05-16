namespace CN88.Features.Site.BetLists.Requests.WagerMonitoring
{
    using Features.Models;
    using MediatR;
    using ViewModels.WagerMonitoring;

    public class WagerMonitoringQuery : WagerMonitoringBaseQuery, IRequest<WagerMonitoringViewModel>
    {
        public WagerMonitoringQuery(IUser viewer) : base(viewer)
        {
            ViewerIdOrViewerAdminId = viewer.IsInternal ? viewer.AdminId : viewer.Id;
        }

        public int ViewerIdOrViewerAdminId { get; }
    }
}