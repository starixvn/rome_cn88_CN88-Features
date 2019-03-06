namespace CN88.Features.Site.BetLists.Requests
{
    using Fanex.BetList.Core.Entities;
    using Features.Models;
    using MediatR;
    using Shared.ViewModels;

    public class GetParlayDetailExcelQuery : GetParlayDetailQuery, IRequest<ExcelFileViewModel>
    {
        public GetParlayDetailExcelQuery(IUser viewer) : base(viewer)
        {
        }

        public RTFHelper? RtfHelper { get; set; }
    }
}