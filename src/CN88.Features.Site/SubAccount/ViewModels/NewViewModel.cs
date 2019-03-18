namespace CN88.Features.Site.SubAccount.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class NewViewModel
    {
        public NewViewModel(string hashKey)
        {
            HashKey = hashKey;
        }

        public string HashKey { get; }

        public IEnumerable<SelectListItem> Suffix1
            => new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" }
            .Select(item => new SelectListItem { Value = item, Text = item, Selected = item == "0" });

        public IEnumerable<SelectListItem> Suffix2
            => new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" }
            .Select(item => new SelectListItem { Value = item, Text = item, Selected = item == "1" });
    }
}