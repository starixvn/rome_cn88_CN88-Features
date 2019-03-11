namespace CN88.Features.Site.Reports.ViewModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Shared.ViewModels.Components;
    using Shared.ViewModels;

    public class BettypeViewModel
    {
        public BettypeViewModel(IEnumerable<SelectListItemExt> betTypes)
        {           
            BetTypeSelectionViewModel = new SingleSelectionViewModel(betTypes, 0);
        }    

        public SingleSelectionViewModel BetTypeSelectionViewModel { get; }
    }
}