namespace CN88.Features.Site.BetLists.Workbooks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using ViewModels;

    public interface ParlayDetailWorkbook
    {
        IWorkbook Workbook { get; }

        Workbook<IEnumerable<ParlayDetailWrapper>> UseTemplate(TemplateInfo templateInfo);

        Workbook<IEnumerable<ParlayDetailWrapper>> Sheet(string sheetName);

        Workbook<IEnumerable<ParlayDetailWrapper>> Title(string titleText);

        Workbook<IEnumerable<ParlayDetailWrapper>> Header(Func<string, string> headerMapping);

        byte[] ExportToBytes();

        ParlayDetailWorkbook SetData(ParlayDetailViewModel comboParlayDetails);

        ParlayDetailWorkbook SetRowMapper(
            Mapper<IEnumerable<ParlayDetailWrapper>> detailsMapper,
            Mapper<IEnumerable<ParlayDetailWrapper>> detailsSubTotalMapper,
            Mapper<IEnumerable<ParlayDetailWrapper>> footerOutstandingMapper,
            Mapper<IEnumerable<ParlayDetailWrapper>> footerTotalWinLostMapper);
    }

    public class ParlayDetailWorkbookImpl : NpoiWorkbook<IEnumerable<ParlayDetailWrapper>>, ParlayDetailWorkbook
    {
        private const int DataSheetIndex = 1;
        private int currentRowIndex;

        public ParlayDetailViewModel ParlayDetails { get; private set; }

        public Row OddRowTemplate { get; private set; }

        public Row EvenRowTemplate { get; private set; }

        public Row SubTotalRowTemplate { get; private set; }

        public Row FooterRowTemplate { get; private set; }

        public Mapper<IEnumerable<ParlayDetailWrapper>> DetailsMapper { get; private set; }

        public Mapper<IEnumerable<ParlayDetailWrapper>> DetailsSubTotalMapper { get; private set; }

        public Mapper<IEnumerable<ParlayDetailWrapper>> FooterOutstandingMapper { get; private set; }

        public Mapper<IEnumerable<ParlayDetailWrapper>> FooterTotalWinLostMapper { get; private set; }

        public ParlayDetailWorkbook SetData(ParlayDetailViewModel comboParlayDetails)
        {
            ParlayDetails = comboParlayDetails;

            return this;
        }

        public ParlayDetailWorkbook SetRowMapper(
            Mapper<IEnumerable<ParlayDetailWrapper>> detailsMapper,
            Mapper<IEnumerable<ParlayDetailWrapper>> detailsSubTotalMapper,
            Mapper<IEnumerable<ParlayDetailWrapper>> footerOutstandingMapper,
            Mapper<IEnumerable<ParlayDetailWrapper>> footerTotalWinLostMapper)
        {
            DetailsMapper = detailsMapper;
            DetailsSubTotalMapper = detailsSubTotalMapper;
            FooterOutstandingMapper = footerOutstandingMapper;
            FooterTotalWinLostMapper = footerTotalWinLostMapper;

            return this;
        }

        protected override Sheet LoadTemplateSheet()
        {
            var sheet = GetSheetAt(TemplateInfo.SheetIndex);

            currentRowIndex = TemplateInfo.BodyStartIndex;

            OddRowTemplate = sheet.GetRow(currentRowIndex++);
            EvenRowTemplate = sheet.GetRow(currentRowIndex++);
            SubTotalRowTemplate = sheet.GetRow(currentRowIndex++);
            FooterRowTemplate = sheet.GetRow(currentRowIndex);

            return GetSheetAt(DataSheetIndex);
        }

        protected override void RemoveSheetAt(int sheetIndex)
            => Workbook.RemoveSheetAt(sheetIndex);

        protected override void FillSheetName(int index, string name)
            => Workbook.SetSheetName(DataSheetIndex, name);

        protected override void FillBody(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            RemoveSheetAt(TemplateInfo.SheetIndex);

            if (ParlayDetails.ParlayDetailWrapperGroups.Any())
            {
                FillBodyRows(sheet);
                FillFooterRows(sheet);
            }
        }

        protected override void FillBodyRows(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            currentRowIndex = TemplateInfo.BodyStartIndex;

            foreach (var detailGroup in ParlayDetails.ParlayDetailWrapperGroups)
            {
                FillDetails(sheet, detailGroup);
                FillSubTotal(sheet, detailGroup);
            }
        }

        protected override void FillFooterRows(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            FillFooterValue(sheet, isTotalWinLost: false);
            FillFooterValue(sheet, isTotalWinLost: true);
        }

        private static void MergeDetailsDateTimeCells(Sheet sheet, int beginRowIndex, int endRowIndex)
        {
            var npoiSheet = sheet as NpoiSheet;
            npoiSheet.Sheet.AddMergedRegion(new CellRangeAddress(beginRowIndex, endRowIndex, 0, 0));
        }

        private void FillFooterValue(Sheet sheet, bool isTotalWinLost)
        {
            var row = sheet.GetRow(currentRowIndex++);
            row.CopyCellsFrom(FooterRowTemplate);
            FillCells(
                row,
                ParlayDetails.ParlayDetailWrappers,
                isTotalWinLost ? FooterTotalWinLostMapper.Mappings : FooterOutstandingMapper.Mappings);
        }

        private void FillDetails(Sheet sheet, IEnumerable<ParlayDetailWrapper> detailGroup)
        {
            var beginRowIndex = currentRowIndex;
            var isEven = false;

            foreach (var detail in detailGroup)
            {
                var row = sheet.GetRow(currentRowIndex++);
                row.CopyCellsFrom(isEven ? EvenRowTemplate : OddRowTemplate);
                isEven = !isEven;
                FillCells(row, new List<ParlayDetailWrapper> { detail }, DetailsMapper.Mappings);
            }

            var endRowIndex = currentRowIndex - 1;
            MergeDetailsDateTimeCells(sheet, beginRowIndex, endRowIndex);
        }

        private void FillSubTotal(Sheet sheet, IEnumerable<ParlayDetailWrapper> detailGroup)
        {
            var row = sheet.GetRow(currentRowIndex++);
            row.CopyCellsFrom(SubTotalRowTemplate);
            FillCells(row, detailGroup, DetailsSubTotalMapper.Mappings);
        }
    }
}