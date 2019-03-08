namespace CN88.Features.Site.CustomerList.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Alpha.Web.Helpers;
    using CN88.Features.CustomerList.Models;
    using CN88.Features.Site.CustomerList.Requests;
    using CN88.Features.Site.Shared.ViewModels.Components;
    using Fanex.BetList.Core.Utils;
    using Features.Shared.Extensions;

    public class MemberStatementViewModel
    {
        public MemberStatementViewModel(
            IEnumerable<MemberStatementModel> statements,
            MemberStatementQuery message,
            IEnumerable<SelectListItem> sportGroups,
            IEnumerable<SelectListItem> fromDates,
            IEnumerable<SelectListItem> toDates)
        {
            Statements = statements;
            TotalStake = statements.Sum(i => i.Stake);
            TotalValidStake = statements.Sum(i => i.ValidStake);
            TotalWinLoss = statements.Sum(i => i.WinLoss);
            SportTypeSelectionViewModel = new SingleSelectionViewModel(sportGroups, message.UserSelectedSportGroupId);
            FromDateSelectionViewModel = new SingleSelectionViewModel(fromDates, message.UserSelectedFromDate);
            ToDateSelectionViewModel = new SingleSelectionViewModel(toDates, message.UserSelectedToDate);
            this.message = message;
        }

        private MemberStatementQuery message;

        public IEnumerable<MemberStatementModel> Statements { get; }

        public SingleSelectionViewModel SportTypeSelectionViewModel { get; }

        public SingleSelectionViewModel FromDateSelectionViewModel { get; }

        public SingleSelectionViewModel ToDateSelectionViewModel { get; }

        private decimal? TotalStake { get; }
        public string TotalStakeStr
        {
            get
            {
                var hasTicket = Statements.Any(i => i.Stake.HasValue);

                return hasTicket ? TotalStake.Value.FormatNumber() : "-";
            }
        }

        private decimal? TotalValidStake { get; }

        public string TotalValidStakeStr
        {
            get
            {
                var hasTicket = Statements.Any(i => i.ValidStake.HasValue);

                return hasTicket ? TotalValidStake.Value.FormatNumber() : "-";
            }
        }

        private decimal? TotalWinLoss { get; }

        public string TotalWinLossStr
        {
            get
            {
                var hasTicket = Statements.Any(i => i.WinLoss.HasValue);

                return hasTicket ? ConvertByBetType.Winloss(TotalWinLoss.Value) : "-";
            }
        }

        public string CustName => message.CustName;

        public int CustId => message.CustId;

        public string BetListOfStatementUrl(DateTime date)
            => Shared.Helpers.UrlHelper.AddQueryStringParamsFunc(
           AppPathHelper.GetAppPath(Features.Shared.Configurations.AppSettings.FeaturesAppPath, "~/BetLists/BetListOfStatement"),
           new Dictionary<string, object>
           {
               ["CustId"] = CustId,
               ["Date"] = date,
               ["SportTypeGroup"] = message.UserSelectedSportGroupId
           });
    }
}