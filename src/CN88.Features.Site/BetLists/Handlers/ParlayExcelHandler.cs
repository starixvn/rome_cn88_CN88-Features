namespace CN88.Features.Site.BetLists.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Features.BetLists;
    using Features.BetLists.Params;
    using Features.BetLists.Resources;
    using Features.Shared.Models;
    using MediatR;
    using NPOI.SS.UserModel;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using Requests;
    using Shared.ViewModels;
    using ViewModels;
    using Workbooks;
    using static ONEbook.SpreadSheets.Column;

    public class ParlayExcelHandler : BetListExcelBaseHandler, IRequestHandler<GetParlayDetailExcelQuery, ExcelFileViewModel>
    {
        private readonly BetListsService service;
        private readonly ITicketHelper ticketHelper;
        private readonly ParlayDetailWorkbook workbook;

        public ParlayExcelHandler(
            BetListsService service,
            ITicketHelper ticketHelper,
            IBetListExcelBuilder excelBuilder,
            ParlayDetailWorkbook workbook) : base(excelBuilder)
        {
            this.service = service;
            this.ticketHelper = ticketHelper;
            this.workbook = workbook ?? new ParlayDetailWorkbookImpl();
        }

        public ExcelFileViewModel Handle(GetParlayDetailExcelQuery message)
        {
            var viewerLevelId = message.ViewerLevelId;
            var parlayDetailViewModel = GetParlayDetail(message, isExportingExcel: true);

            var onebookWorkbook = workbook
                .SetData(parlayDetailViewModel)
                .SetRowMapper(
                    DetailsMapper(),
                    DetailsSubTotalMapper(parlayDetailViewModel),
                    FooterMapper(parlayDetailViewModel, isTotalWinLost: false),
                    FooterMapper(parlayDetailViewModel, isTotalWinLost: true))
                .UseTemplate(GetTemplateInfo(Enumeration.FromValue<UserLevel>(viewerLevelId)));

            RtfHelper = message.RtfHelper ?? InitRTFHelper((onebookWorkbook as NpoiWorkbook<IEnumerable<ParlayDetailWrapper>>).Workbook);

            var fileContent = onebookWorkbook
                .Sheet(parlayDetailViewModel.Title)
                .Title(parlayDetailViewModel.Title)
                .Header(BetListsResources.ResourceManager.GetString)
                .ExportToBytes();

            return new ExcelFileViewModel("SystemParlay_Details", fileContent);
        }

        protected internal override TemplateInfo GetTemplateInfo(UserLevel viewerLevel)
            => new TemplateInfo(
                  GetExcelTemplatePath("ParlayDetail"),
                  noInfomationText: BetListsResources.NoInformationIsAvailable,
                  bodyStartIndex: 3,
                  bodyEndIndex: 5,
                  footerEndIndex: 7);

        private ParlayDetailViewModel GetParlayDetail(GetParlayDetailExcelQuery message, bool isExportingExcel = false)
        {
            var param = new GetParlayDetailParam(
                message.RefNo,
                message.TransId,
                message.BetId,
                message.CustId,
                message.ActorLevel.IsHeadline() ? message.ViewerId : 0);

            var parlayDetailData = service.GetParlayDetails(param);

            return new ParlayDetailViewModel(message, parlayDetailData, ticketHelper, false, isExportingExcel);
        }

        private Mapper<IEnumerable<ParlayDetailWrapper>> DetailsMapper()
        {
            var mapper = new Mapper<IEnumerable<ParlayDetailWrapper>>();

            return mapper.Map(A, details => details.First().WinLostDate)
                .Map(B, details => RenderEvent(details.First()))
                .Map(C, details => RenderNumber(details.First().Odds))
                .Map(D, details => details.First().Stake)
                .Map(E, details => RenderWinLossComm(details.First()))
                .Map(F, details => details.First().Status);
        }

        private IRichTextString RenderEvent(ParlayDetailWrapper detail)
        {
            for (var i = 0; i < detail.Events.Count; i++)
            {
                RtfHelper.RTFRenderer.AddText((i > 0 ? "\n" : string.Empty) + detail.Events[i]);
            }

            var eventRtfText = RtfHelper.RTFRenderer.Render();
            RtfHelper.RTFRenderer.Clear();

            return eventRtfText;
        }

        private IRichTextString RenderWinLossComm(ParlayDetailWrapper detail)
        {
            var rtfNumber = new RTFNumber(RtfHelper.RTFRenderer, RtfHelper.PosFont, RtfHelper.NegFont);

            rtfNumber.Render(detail.WinLost + "\n");
            rtfNumber.Render(detail.Comm);

            var rtfText = RtfHelper.RTFRenderer.Render();
            RtfHelper.RTFRenderer.Clear();

            return rtfText;
        }

        private IRichTextString RenderNumber(string numberValue)
        {
            var rtfNumber = new RTFNumber(RtfHelper.RTFRenderer, RtfHelper.PosFont, RtfHelper.NegFont);

            rtfNumber.Render(numberValue);

            var rtfText = RtfHelper.RTFRenderer.Render();
            RtfHelper.RTFRenderer.Clear();

            return rtfText;
        }

        private Mapper<IEnumerable<ParlayDetailWrapper>> DetailsSubTotalMapper(ParlayDetailViewModel viewModel)
        {
            var mapper = new Mapper<IEnumerable<ParlayDetailWrapper>>();

            return mapper
                .Map(A, _ => BetListsResources.SubTotal + ":")
                .Map(E, details => RenderNumber(viewModel.SubTotalWinLost(details)));
        }

        private Mapper<IEnumerable<ParlayDetailWrapper>> FooterMapper(ParlayDetailViewModel viewModel, bool isTotalWinLost)
        {
            var mapper = new Mapper<IEnumerable<ParlayDetailWrapper>>();

            mapper
                .Map(A, _ => (isTotalWinLost ? BetListsResources.Total : BetListsResources.Outstanding) + ":")
                .Map(E, _ => RenderNumber(isTotalWinLost ? viewModel.TotalWinLost : viewModel.OutStanding));

            return mapper;
        }
    }
}