namespace CN88.Features.Site.BetLists.Requests
{
    using Features.Models;
    using MediatR;
    using Site.Requests;
    using ViewModels;

    public class GetSubTicketsQuery : BaseQuery, IRequest<SubTicketsViewModel>
    {
        public GetSubTicketsQuery(IUser viewer) : base(viewer)
        {
            DisplayVNIP = viewer.IsAdminCanViewVNIP;
            ViewerIdOrViewerAdminId = viewer.IsInternal ? viewer.AdminId : viewer.Id;
        }

        public long TransId { get; }

        public bool DisplayVNIP { get; }

        public int ViewerIdOrViewerAdminId { get; }
    }
}