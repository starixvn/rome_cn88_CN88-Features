namespace CN88.Features.Site.BetLists.Requests
{
    using Fanex.BetList.Core.Entities;
    using Features.Models;
    using MediatR;
    using Shared.ViewModels;

    public class BetListOfReportExcelQuery : BetListOfReportQuery, IRequest<ExcelFileViewModel>
    {
        public BetListOfReportExcelQuery(IUser viewer) : base(viewer)
        {
        }

        public RTFHelper? RtfHelper { get; set; }
    }
}