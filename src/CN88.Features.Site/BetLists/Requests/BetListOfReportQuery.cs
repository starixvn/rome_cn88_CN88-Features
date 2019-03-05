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

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int SportTypeGroup { get; }

        public int BetTypeGroup { get; }

        public int LiveIndicator { get; }

        public int Type { get; }

        public bool DisplayVNIP { get; }

        public int ViewerIdOrViewerAdminId { get; }
    }
}