namespace CN88.Features.Site.BetLists.ViewModels.WagerMonitoring
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class MultipleSelectViewModel
    {
        public MultipleSelectViewModel(IEnumerable<SelectListItem> items, bool isSelectAll)
        {
            Items = items;
            IsSelectAll = isSelectAll;
        }

        public IEnumerable<SelectListItem> Items { get; }

        public bool IsSelectAll { get; }
    }
}