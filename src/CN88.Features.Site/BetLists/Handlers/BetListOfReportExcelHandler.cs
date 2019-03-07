namespace CN88.Features.Site.BetLists.Handlers
{
    using System.Threading.Tasks;
    using Alpha.SmartPunter.Services;
    using BetList.Core.Builders;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Features.BetLists;
    using Features.BetLists.Params;
    using Features.BetLists.Resources;
    using Features.Shared.Models;
    using MediatR;
    using ONEbook.SpreadSheets;
    using Requests;
    using Shared.ViewModels;
    using ViewModels;
    using Workbooks;

    public class BetListOfReportExcelHandler :
        BetListExcelBaseHandler,
        IAsyncRequestHandler<BetListOfReportExcelQuery, ExcelFileViewModel>
    {
        private BetListsService service;
        private ISmartPunterBetListMonitor monitorService;
        private ITicketHelper ticketHelper;
        private IBetListCN88HtmlBuilder cn88Builder;
        private IBetListHTMLBuilder builder;

        private BetListOfReportWorkbook workbook;

        public BetListOfReportExcelHandler(
            IBetListExcelBuilder excelBuilder,
            BetListsService service,
            ISmartPunterBetListMonitor monitorService,
            ITicketHelper ticketHelper,
            IBetListCN88HtmlBuilder cn88Builder,
            IBetListHTMLBuilder builder,
            BetListOfReportWorkbook workbook) : base(excelBuilder)
        {
            this.service = service;
            this.monitorService = monitorService;
            this.ticketHelper = ticketHelper;
            this.cn88Builder = cn88Builder;
            this.builder = builder;

            this.workbook = workbook;
        }

        public async Task<ExcelFileViewModel> Handle(BetListOfReportExcelQuery message)
        {
            var viewerLevel = Enumeration.FromValue<UserLevel>(message.ViewerLevelId);
            var param = new BetListOfReportParam(
                message.CustId,
                message.FromDate,
                message.ToDate,
                message.SportTypeGroup,
                message.BetTypeGroup,
                message.LiveIndicator,
                message.Type);

            var betlistData = await service.GetBetListOfReport(param);
            var viewModel =
                new BetListOfReportViewModel(message, betlistData, ticketHelper, cn88Builder, builder);

            var onebookWorkbook = workbook
                .SetData(viewModel)
                .UseTemplate(GetTemplateInfo(viewerLevel));

            RtfHelper = message.RtfHelper ?? InitRTFHelper(workbook.Workbook);

            var fileContent = onebookWorkbook
                .Header(BetListsResources.ResourceManager.GetString)
                .Title(viewModel.ReportTitle)
                .ExportToBytes();

            return new ExcelFileViewModel(viewModel.ReportTitle, fileContent);
        }

        protected internal override TemplateInfo GetTemplateInfo(UserLevel viewerLevel)
            => new TemplateInfo(
                  GetExcelTemplatePath($"BetListOfReport_{viewerLevel.DisplayName}"),
                  noInfomationText: BetListsResources.NoInformationIsAvailable,
                  bodyStartIndex: 3,
                  bodyEndIndex: 4,
                  footerEndIndex: 5);
    }
}