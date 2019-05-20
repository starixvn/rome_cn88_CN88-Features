namespace CN88.Features.Site.BetLists.Handlers
{
    using System.Collections.Generic;
    using Alpha.SmartPunter.Services;
    using BetList.Core.Builders;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Features.BetLists;
    using Features.BetLists.Models;
    using Features.BetLists.Params;
    using Features.BetLists.Resources;
    using Features.Extensions;
    using Features.Shared.Models;
    using MediatR;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using Requests;
    using Shared.ViewModels;
    using ViewModels;
    using Workbooks;

    public class BetListOfReportExcelHandler :
        BetListExcelBaseHandler,
        IRequestHandler<BetListOfReportExcelQuery, ExcelFileViewModel>
    {
        private readonly BetListsService service;
        private readonly ITicketHelper ticketHelper;
        private readonly IBetListCN88HtmlBuilder cn88Builder;
        private readonly IBetListHTMLBuilder builder;

        private readonly BetListOfReportWorkbook workbook;
        private readonly IBetListCN88ExcelBuilder cn88ExcelBuilder;

        public BetListOfReportExcelHandler(
            IBetListExcelBuilder excelBuilder,
            IBetListCN88ExcelBuilder cn88ExcelBuilder,
            BetListsService service,
            ITicketHelper ticketHelper,
            IBetListCN88HtmlBuilder cn88Builder,
            IBetListHTMLBuilder builder,
            BetListOfReportWorkbook workbook) : base(excelBuilder)
        {
            this.service = service;
            this.ticketHelper = ticketHelper;
            this.cn88Builder = cn88Builder;
            this.builder = builder;

            this.workbook = workbook;
            this.cn88ExcelBuilder = cn88ExcelBuilder;
        }

        public ExcelFileViewModel Handle(BetListOfReportExcelQuery message)
        {
            var viewerLevel = Enumeration.FromValue<UserLevel>(message.ViewerLevelId);
            var param = new BetListOfReportParam(
                message.CustId,
                message.FromDate,
                message.ToDate,
                message.UserSelectedSportGroupId,
                message.UserSelectedBetTypeId,
                message.UserSelectedLiveStatusId,
                message.Type,
                message.LimitedDateToShowInfo);

            var betlistData = service.GetBetListOfReport(param);
            var viewModel =
                new BetListOfReportViewModel(message, betlistData, ticketHelper, cn88Builder, builder);

            var onebookWorkbook = workbook
                .SetData(viewModel, ExcelBuilder, cn88ExcelBuilder)
                .UseTemplate(GetTemplateInfo(viewerLevel));

            RtfHelper = message.RtfHelper ?? InitRTFHelper((onebookWorkbook as NpoiWorkbook<IEnumerable<TicketDto>>).Workbook);
            workbook.SetRTFHelper(RtfHelper);

            var title = $"{BetListsResources.MemberBetList.ToUpperInvariant()} {message.CustName}|{message.FromDate.ToShortDateFormat()} --> {message.ToDate.ToShortDateFormat()}";
            var fileContent = onebookWorkbook
                .Header(BetListsResources.ResourceManager.GetString)
                .Title(title)
                .ExportToBytes();

            var fileName = $"{BetListsResources.BetList.ToUpperInvariant()} - {message.CustName} - {message.FromDate.ToShortDateFormat()} -- {message.ToDate.ToShortDateFormat()}";

            return new ExcelFileViewModel(fileName, fileContent);
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