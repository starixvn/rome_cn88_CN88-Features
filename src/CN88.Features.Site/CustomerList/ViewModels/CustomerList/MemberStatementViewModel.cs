namespace CN88.Features.Site.CustomerList.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using CN88.Features.CustomerList.Models;
    using CN88.Features.Site.CustomerList.Requests;
    using CN88.Features.Site.Shared.ViewModels.Components;
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
            CustName = message.CustName;
        }

        public IEnumerable<MemberStatementModel> Statements { get; }

        public MemberStatementQuery Message { get; }

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

                return hasTicket ? TotalWinLoss.Value.FormatNumber() : "-";
            }
        }

        public string CustName { get; }
    }
}