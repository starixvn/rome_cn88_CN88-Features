namespace CN88.Features.Site.Shared.ViewModels.Components
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class SportGroupSelectionViewModel
    {
        public SportGroupSelectionViewModel(
            IEnumerable<SelectListItem> sportGroups,
            int userSelectedSportGroupId)
        {
            SportGroups = sportGroups;
            UserSelectedSportGroupId = userSelectedSportGroupId;
        }

        public IEnumerable<SelectListItem> SportGroups { get; }

        public int UserSelectedSportGroupId { get; }
    }
}