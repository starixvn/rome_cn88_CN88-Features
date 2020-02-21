namespace CN88.Features.Site.BetLists.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using BetList.Core.Builders;
    using CN88.Features.Reports.Services;
    using CN88.Features.Shared.Extensions;
    using CN88.Features.Shared.Services;
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
        IRequestHandler<BetListOfReportExcelQuery, ExcelFileViewModel>,
        IRequestHandler<BetListOfEBReportExcelQuery, ExcelFileViewModel>
    {
        private readonly BetListsService service;
        private readonly ITicketHelper ticketHelper;
        private readonly IBetListCN88HtmlBuilder cn88Builder;
        private readonly IBetListHTMLBuilder builder;
        private readonly SportGroupService sportGroupService;

        private readonly BetListOfReportWorkbook workbook;
        private readonly IBetListCN88ExcelBuilder cn88ExcelBuilder;
        private readonly AccountingPeriodService accountingPeriodService;

        public BetListOfReportExcelHandler(
            IBetListExcelBuilder excelBuilder,
            IBetListCN88ExcelBuilder cn88ExcelBuilder,
            BetListsService service,
            ITicketHelper ticketHelper,
            IBetListCN88HtmlBuilder cn88Builder,
            IBetListHTMLBuilder builder,
            AccountingPeriodService accountingPeriodService,
            SportGroupService sportGroupService,

            BetListOfReportWorkbook workbook) : base(excelBuilder)
        {
            this.service = service;
            this.sportGroupService = sportGroupService;
            this.ticketHelper = ticketHelper;
            this.cn88Builder = cn88Builder;
            this.builder = builder;

            this.workbook = workbook;
            this.cn88ExcelBuilder = cn88ExcelBuilder;
            this.accountingPeriodService = accountingPeriodService;
        }

        public ExcelFileViewModel Handle(BetListOfReportExcelQuery message)
        {
            var sportGroupIds = sportGroupService.GetSportGroupIdsByType(message.UserSelectedSportGroupId);

            return BuildExcelFileViewModel(message, sportGroupIds.JoinWithComma(), message.RtfHelper);
        }

        public ExcelFileViewModel Handle(BetListOfEBReportExcelQuery message)
            => BuildExcelFileViewModel(message, message.UserSelectedSportGroupIds, message.RtfHelper);

        protected internal override TemplateInfo GetTemplateInfo(UserLevel viewerLevel)
            => new TemplateInfo(
                  GetExcelTemplatePath($"BetListOfReport_{viewerLevel.DisplayName}"),
                  noInfomationText: BetListsResources.NoInformationIsAvailable,
                  bodyStartIndex: 3,
                  bodyEndIndex: 4,
                  footerEndIndex: 5);

        private ExcelFileViewModel BuildExcelFileViewModel(
            BetListOfReportQuery message,
            string sportTypeGroupIds,
            RTFHelper? rtfHelper)
        {
            var param = new BetListOfReportParam(
                message.CustId,
                message.FromDate,
                message.ToDate,
                sportTypeGroupIds,
                message.UserSelectedBetTypeId,
                message.UserSelectedLiveStatusId,
                message.Type,
                message.LimitedDateToShowInfo,
                message.ActorLevel.IsHeadline() ? message.ViewerId : 0);

            var oldAccountingPeriods = message.IsHeadline ?
                accountingPeriodService.GetOldAccountingPeriodsForHeadline(message.ViewerId) :
                accountingPeriodService.GetOldAccountingPeriodsForAgency();

            var betlistData = message.IsValidReportRange(oldAccountingPeriods) ?
                service.GetBetListOfReport(param) : new BetListData(Enumerable.Empty<TicketDto>(), new List<ITicketData>());
            var viewModel =
                new BetListOfReportViewModel(message, betlistData, ticketHelper, cn88Builder, builder);

            var onebookWorkbook = workbook
                .SetData(viewModel, ExcelBuilder, cn88ExcelBuilder)
                .UseTemplate(GetTemplateInfo(message.ActorLevel));

            RtfHelper = rtfHelper ?? InitRTFHelper((onebookWorkbook as NpoiWorkbook<IEnumerable<TicketDto>>).Workbook);
            workbook.SetRTFHelper(RtfHelper);

            var title = $"{BetListsResources.MemberBetList.ToUpperInvariant()} {message.CustName}|{message.FromDate.ToShortDateFormat()} --> {message.ToDate.ToShortDateFormat()}";
            var fileContent = onebookWorkbook
                .Header(BetListsResources.ResourceManager.GetString)
                .Title(title)
                .ExportToBytes();

            var fileName = $"{BetListsResources.BetList.ToUpperInvariant()} - {message.CustName} - {message.FromDate.ToShortDateFormat()} -- {message.ToDate.ToShortDateFormat()}";

            return new ExcelFileViewModel(fileName, fileContent);
        }
    }
}