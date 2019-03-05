namespace CN88.Features.Site.BetLists.Requests
{
    using System;
    using Features.Models;
    using MediatR;
    using Site.Requests;
    using ViewModels;

    public class BetListOfReportQuery : BaseQuery, IRequest<BetListOfReportViewModel>
    {
        public BetListOfReportQuery(IUser viewer) : base(viewer)
        {
            DisplayVNIP = viewer.IsAdminCanViewVNIP;
            ViewerIdOrViewerAdminId = viewer.IsInternal ? viewer.AdminId : viewer.Id;
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int SportTypeGroup { get; set; }

        public int BetTypeGroup { get; set; }

        public int LiveIndicator { get; set; }

        public int Type { get; set; }

        public bool DisplayVNIP { get; }

        public int ViewerIdOrViewerAdminId { get; }
    }
}