namespace CN88.Features.Site.Reports.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CN88.Features.Reports.Models;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;

    public interface ReportsWorkbook
    {
        Workbook<ReportModel> UseTemplate(TemplateInfo templateInfo);

        Workbook<ReportModel> Sheet(string sheetName);

        Workbook<ReportModel> Header(Func<string, string> headerMapping);

        byte[] ExportToBytes();

        ReportsWorkbook SetData(IEnumerable<ReportModel> reportData, ReportModel total);

        ReportsWorkbook SetRowMappers(Mapper<ReportModel> bodyRowMapper, Mapper<ReportModel> totalRowMapper);
    }

    public class ReportsWorkbookImpl : NpoiWorkbook<ReportModel>, ReportsWorkbook
    {
        private const int ReportSheetIndex = 1;

        private int currentRowIndex;

        internal IEnumerable<ReportModel> ReportData { get; set; }

        internal ReportModel Total { get; set; }

        internal Mapper<ReportModel> BodyRowMapper { get; set; }

        internal Mapper<ReportModel> TotalRowMapper { get; set; }

        internal Row ReportRowTemplate { get; set; }

        public ReportsWorkbook SetData(IEnumerable<ReportModel> reportData, ReportModel total)
        {
            ReportData = reportData;
            Total = total;

            return this;
        }

        public ReportsWorkbook SetRowMappers(Mapper<ReportModel> bodyRowMapper, Mapper<ReportModel> totalRowMapper)
        {
            BodyRowMapper = bodyRowMapper;
            TotalRowMapper = totalRowMapper;

            return this;
        }

        protected override void FillBody(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            RemoveTemplateRows(sheet);

            if (ReportData.Any())
            {
                FillBodyRows(sheet);
                FillFooterRows(sheet);
            }
            else
            {
                FillNoInfoRow(sheet);
            }
        }

        protected override Sheet LoadTemplateSheet()
        {
            var sheet = GetSheetAt(TemplateInfo.SheetIndex);

            currentRowIndex = TemplateInfo.BodyStartIndex;
            ReportRowTemplate = sheet.GetRow(currentRowIndex++);

            var reportSheet = GetSheetAt(ReportSheetIndex);

            return reportSheet;
        }

        protected override void RemoveTemplateRows(Sheet sheet)
        {
            Workbook.RemoveSheetAt(TemplateInfo.SheetIndex);
        }

        protected override void FillBodyRows(Sheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet), "Sheet cannot be null");
            }

            currentRowIndex = TemplateInfo.BodyStartIndex;

            var totalRow = sheet.GetRow(currentRowIndex++);
            totalRow.CopyCellsFrom(ReportRowTemplate);
            FillCells(totalRow, Total, TotalRowMapper.Mappings);

            foreach (var data in ReportData)
            {
                var row = sheet.GetRow(currentRowIndex++);
                row.CopyCellsFrom(ReportRowTemplate);
                FillCells(row, data, BodyRowMapper.Mappings);
            }
        }
    }
}