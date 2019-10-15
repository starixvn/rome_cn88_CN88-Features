namespace CN88.Features.Site.BetLists.Requests
{
    using Fanex.BetList.Core.Entities;
    using Features.Models;
    using MediatR;
    using Shared.ViewModels;

    public class BetListOfEBReportExcelQuery : BetListOfReportQuery, IRequest<ExcelFileViewModel>
    {
        public BetListOfEBReportExcelQuery(IUser viewer) : base(viewer)
        {
        }

        public RTFHelper? RtfHelper { get; set; }

        public string UserSelectedSportGroupIds { get; set; }
    }
}