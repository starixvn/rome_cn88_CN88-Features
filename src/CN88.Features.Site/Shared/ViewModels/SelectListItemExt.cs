namespace CN88.Features.Site.Shared.ViewModels
{
    using System.Web.Mvc;

    public class SelectListItemExt : SelectListItem
    {
        public int? ProductId { get; set; }

        public int SportTypeGroup { get; set; }
    }
}