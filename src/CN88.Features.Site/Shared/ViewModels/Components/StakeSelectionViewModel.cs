namespace CN88.Features.Site.Shared.ViewModels.Components
{
    using System.Collections.Generic;

    public class StakeSelectionViewModel
    {
        public StakeSelectionViewModel(
            IEnumerable<SelectListItemExt> items,
            int stakeMode,
            decimal stakeLimit)
        {
            Items = items;
            StakeMode = stakeMode;
            StakeLimit = stakeLimit;
        }

        public IEnumerable<SelectListItemExt> Items { get; }

        public int StakeMode { get; }

        public decimal StakeLimit { get; }
    }
}