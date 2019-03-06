namespace CN88.Features.Site.BetLists.Requests
{
    using Features.Models;
    using MediatR;
    using Site.Requests;
    using ViewModels;

    public class GetParlaySubTicketsQuery : BaseQuery, IRequest<ParlaySubTicketsViewModel>
    {
        public GetParlaySubTicketsQuery(IUser viewer) : base(viewer)
        {
            DisplayVNIP = viewer.IsAdminCanViewVNIP;
            ViewerIdOrViewerAdminId = viewer.IsInternal ? viewer.AdminId : viewer.Id;
        }

        public string BetCheck { get; set; }

        public string BetTeam { get; set; }

        public decimal Handicap1 { get; set; }

        public decimal Handicap2 { get; set; }

        public string TransId { get; set; }

        public bool DisplayVNIP { get; }

        public int ViewerIdOrViewerAdminId { get; }
    }
}