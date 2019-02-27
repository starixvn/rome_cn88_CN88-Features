namespace CN88.Features.Site.Shared
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Features.Shared.AppResources;
    using Features.Shared.Models;

    public class BaseHandler
    {
        private const string ExcelTemplateDir = "~/App_Data/ExcelTemplates/{0}/{1}.{2}";
        private Func<string, string> mapPath;

        protected internal Func<string, string> MapPath
        {
            get { return mapPath ?? HttpContext.Current.Server.MapPath; }
            set { mapPath = value; }
        }

        internal Func<string, bool> IsFileExist { get; set; } = File.Exists;

        protected internal IEnumerable<SelectListItem> BuildSportGroupSelectListItems(
            IEnumerable<SportGroup> sportGroups,
            int userSelectedSportGroupId)
        {
            var items = sportGroups.Select(sportGroup => new SelectListItem
            {
                Text = sportGroup.DisplayName,
                Value = sportGroup.Id.ToString(),
                Selected = sportGroup.Id == userSelectedSportGroupId
            }).ToList();

            items.Insert(0, new SelectListItem { Text = SharedResources.AllSports, Value = "0", Selected = userSelectedSportGroupId == 0 });

            return items;
        }

        protected internal IEnumerable<SelectListItem> BuildBetTypeSelectListItems(
            IEnumerable<BetType> betTypes,
            int userSelectedBetTypeId)
        {
            var items = betTypes.Select(betType => new SelectListItem
            {
                Text = betType.DisplayName,
                Value = betType.Id.ToString(),
                Selected = betType.Id == userSelectedBetTypeId
            }).ToList();

            items.Insert(0, new SelectListItem { Text = SharedResources.AllBetTypes, Value = "0", Selected = userSelectedBetTypeId == 0 });

            return items;
        }

        protected internal virtual string GetExcelTemplatePath(string templateName, string fileExtension = "xls")
        {
            if (string.IsNullOrWhiteSpace(templateName))
            {
                throw new ArgumentNullException(nameof(templateName), "Template Name cannot be null or empty");
            }

            var relativePath = string.Format(ExcelTemplateDir, GetExcelTemplateFolder(), templateName, fileExtension);

            var absolutePath = MapPath(relativePath);

            if (IsFileExist(absolutePath))
            {
                return absolutePath;
            }

            throw new FileNotFoundException($"Template {absolutePath} does not exist");
        }

        protected internal virtual string GetExcelTemplateFolder()
        {
            var reportNamespace = GetType().Namespace;

            var lastIndexOfDot = reportNamespace.LastIndexOf(".", StringComparison.Ordinal) + 1;

            if (lastIndexOfDot <= 0)
            {
                throw new ArgumentException("Namespace does not contain any dot character, wrong report convention.");
            }

            var templateFolderName = reportNamespace.Substring(lastIndexOfDot);

            return templateFolderName;
        }
    }
}