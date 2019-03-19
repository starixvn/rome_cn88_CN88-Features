namespace CN88.Features.Site.BetLists.Requests
{
    using System;
    using Features.Models;
    using Features.Shared.Constants;
    using MediatR;
    using Site.Requests;
    using ViewModels;

    public class BetListOfStatementQuery : BaseQuery, IRequest<BetListOfStatementViewModel>
    {
        public BetListOfStatementQuery(IUser viewer) : base(viewer)
        {
            DisplayVNIP = viewer.IsAdminCanViewVNIP;
            ViewerIdOrViewerAdminId = viewer.IsInternal ? viewer.AdminId : viewer.Id;
            PageIndex = PagerConstant.DefaultPageIndex;
            PageSize = PagerConstant.DefaultPageSize;
        }

        public DateTime Date { get; set; }

        public int SportTypeGroup { get; set; }

        public bool DisplayVNIP { get; }

        public int ViewerIdOrViewerAdminId { get; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}