namespace CN88.Features.Site.BetLists.Handlers
{
    using System;
    using System.IO;
    using System.Web.Hosting;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Features.BetLists.Resources;
    using Features.Shared.Models;
    using NPOI.SS.UserModel;
    using ONEbook.SpreadSheets;

    public class BetListExcelBaseHandler
    {
        private const string ExcelTemplateDir = "~/BetLists/ExcelTemplates/{0}.{1}";
        private const string FontTahoma = "Tahoma";
        private Func<string, string> mapPath;

        public BetListExcelBaseHandler(IBetListExcelBuilder excelBuilder)
        {
            ExcelBuilder = excelBuilder;
            RtfHelper = excelBuilder.RTFHelper;
        }

        protected internal IBetListExcelBuilder ExcelBuilder { get; }

        protected internal RTFHelper RtfHelper { get; set; }

        protected internal Func<string, string> MapPath
        {
            get { return mapPath ?? HostingEnvironment.MapPath; }
            set { mapPath = value; }
        }

        internal Func<string, bool> IsFileExist { get; set; } = File.Exists;

        protected internal static RTFHelper InitRTFHelper(IWorkbook workbook)
        {
            var rtfHelper = new RTFHelper
            {
                NegFont = workbook.CreateFont(),
                NormalFont = workbook.CreateFont(),
                PosFont = workbook.CreateFont(),
                NegFontCrossed = workbook.CreateFont(),
                NormalFontCrossed = workbook.CreateFont(),
                PosFontCrossed = workbook.CreateFont(),
                RTFRenderer = new RtfTextRender()
            };

            InitRTFHelperFonts(rtfHelper);

            return rtfHelper;
        }

        protected internal virtual string GetExcelTemplatePath(string templateName, string fileExtension = "xls")
        {
            if (string.IsNullOrWhiteSpace(templateName))
            {
                throw new ArgumentNullException(nameof(templateName), "Template Name cannot be null or empty");
            }

            var relativePath = string.Format(ExcelTemplateDir, templateName, fileExtension);

            var absolutePath = MapPath(relativePath);

            if (IsFileExist(absolutePath))
            {
                return absolutePath;
            }

            throw new FileNotFoundException($"Template {absolutePath} does not exist");
        }

        protected internal string GetExcelTemplateFolder()
        {
            var handlerNamespace = GetType().Namespace;

            var lastIndexOfDot = handlerNamespace.LastIndexOf(".", StringComparison.Ordinal) + 1;

            if (lastIndexOfDot <= 0)
            {
                throw new ArgumentException("Namespace does not contain any dot character, wrong report convention.");
            }

            var templateFolderName = handlerNamespace.Substring(lastIndexOfDot);

            return templateFolderName;
        }

        protected internal virtual TemplateInfo GetTemplateInfo(UserLevel viewerLevel)
            => new TemplateInfo(
                   ExcelTemplatePath(viewerLevel),
                   noInfomationText: BetListsResources.NoInformationIsAvailable,
                   bodyStartIndex: 3,
                   bodyEndIndex: 4,
                   footerEndIndex: 5);

        protected internal virtual string ExcelTemplatePath(UserLevel viewerLevel)
            => GetExcelTemplatePath(viewerLevel.DisplayName);

        private static void InitRTFHelperFonts(RTFHelper rtfHelper)
        {
            rtfHelper.PosFont.Boldweight = (short)FontBoldWeight.Bold;
            rtfHelper.PosFontCrossed.Boldweight = (short)FontBoldWeight.Bold;

            rtfHelper.NegFont.Boldweight = (short)FontBoldWeight.Bold;
            rtfHelper.NegFontCrossed.Boldweight = (short)FontBoldWeight.Bold;

            rtfHelper.NormalFont.Boldweight = (short)FontBoldWeight.Normal;
            rtfHelper.NormalFontCrossed.Boldweight = (short)FontBoldWeight.Normal;

            rtfHelper.NegFont.FontName = FontTahoma;
            rtfHelper.NegFontCrossed.FontName = FontTahoma;
            rtfHelper.NormalFont.FontName = FontTahoma;
            rtfHelper.NormalFontCrossed.FontName = FontTahoma;
            rtfHelper.PosFont.FontName = FontTahoma;
            rtfHelper.PosFontCrossed.FontName = FontTahoma;

            rtfHelper.NegFont.FontHeightInPoints = 9;
            rtfHelper.NegFontCrossed.FontHeightInPoints = 9;
            rtfHelper.NormalFont.FontHeightInPoints = 9;
            rtfHelper.NormalFontCrossed.FontHeightInPoints = 9;
            rtfHelper.PosFont.FontHeightInPoints = 9;
            rtfHelper.PosFontCrossed.FontHeightInPoints = 9;

            rtfHelper.NormalFontCrossed.IsStrikeout = true;
            rtfHelper.PosFontCrossed.IsStrikeout = true;
            rtfHelper.NegFontCrossed.IsStrikeout = true;

            rtfHelper.NegFont.Color = (short)FontColor.Red;
            rtfHelper.NegFontCrossed.Color = (short)FontColor.Red;
        }
    }
}