namespace CN88.Features.Site.Reports.ViewModels
{
    using System.Collections.Generic;
    using Shared.ViewModels;
    using Shared.ViewModels.Components;

    public class BettypeViewModel
    {
        public BettypeViewModel(IEnumerable<SelectListItemExt> betTypes)
        {
            BetTypeSelectionViewModel = new SingleSelectionViewModel(betTypes, 0);
        }

        public SingleSelectionViewModel BetTypeSelectionViewModel { get; }
    }
}