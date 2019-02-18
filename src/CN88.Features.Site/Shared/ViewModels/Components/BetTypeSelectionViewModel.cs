namespace CN88.Features.Site.Shared.ViewModels.Components
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class BetTypeSelectionViewModel
    {
        public BetTypeSelectionViewModel(
            IEnumerable<SelectListItem> betTypes,
            int userSelectedBetTypeId)
        {
            BetTypes = betTypes;
            UserSelectedBetTypeId = userSelectedBetTypeId;
        }

        public IEnumerable<SelectListItem> BetTypes { get; }

        public int UserSelectedBetTypeId { get; }
    }
}