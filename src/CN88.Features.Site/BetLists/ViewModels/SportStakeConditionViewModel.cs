namespace CN88.Features.Site.BetLists.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Features.Shared.AppResources;
    using Features.Shared.Constants;
    using Models;

    public class SportStakeConditionViewModel
    {
        private static readonly IEnumerable<SelectListItem> stackModes =
            StakeModeConstant.Modes.Select(
                mode =>
                new SelectListItem
                {
                    Text = SharedResources.ResourceManager.GetString($"StakeMode_{mode}"),
                    Value = mode.ToString()
                });

        public SportStakeConditionViewModel(
            bool isSelectAllSports,
            int allSportsStakeMode,
            decimal allSportsStakeLimit,
            IEnumerable<SportStakeCondition> perSportsStakeConditions)
        {
            IsSelectAllSports = isSelectAllSports;
            AllSportsStakeMode = allSportsStakeMode;
            AllSportsStakeLimit = allSportsStakeLimit;
            PerSportsStakeConditions = perSportsStakeConditions;
        }

        public bool IsSelectAllSports { get; }

        public int AllSportsStakeMode { get; }

        public decimal AllSportsStakeLimit { get; }

        public IEnumerable<SportStakeCondition> PerSportsStakeConditions { get; }

        public IEnumerable<SelectListItem> StakeModes => stackModes;
    }
}