namespace CN88.Features.Site.CustomerList.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using CN88.Features.CustomerList.Models;
    using CN88.Features.Site.CustomerList.Requests;
    using CN88.Features.Site.Shared.ViewModels.Components;
    using Features.Shared.Utils;
    using CN88.Features.Extensions;
    public class MemberStatementViewModel
    {
        public MemberStatementViewModel(
            IEnumerable<MemberStatementModel> statements,
            MemberStatementQuery message,
            IEnumerable<SelectListItem> sportGroups)
        {
            Statements = statements;
            TotalStake = statements.Where(i => i.Stake.HasValue).Sum(i => i.Stake);
            TotalValidStake = statements.Where(i => i.ValidStake.HasValue).Sum(i => i.ValidStake);
            TotalWinLoss = statements.Where(i => i.WinLoss.HasValue).Sum(i => i.WinLoss);
            SportTypeSelectionViewModel = new SingleSelectionViewModel(sportGroups, 0);

            var dates = new List<SelectListItem> { new SelectListItem { Text = SystemTime.Today.ToShortDateFormat(), Value = SystemTime.Today.ToString() } };
            FromDateSelectionViewModel = new SingleSelectionViewModel(dates, 0);
            ToDateSelectionViewModel = new SingleSelectionViewModel(dates, 0);
            UserName = message.CustName;
        }

        public IEnumerable<MemberStatementModel> Statements { get; }

        public MemberStatementQuery Message { get; }

        public SingleSelectionViewModel SportTypeSelectionViewModel { get; }

        public SingleSelectionViewModel FromDateSelectionViewModel { get; }

        public SingleSelectionViewModel ToDateSelectionViewModel { get; }

        public decimal? TotalStake { get; }

        public decimal? TotalValidStake { get; }

        public decimal? TotalWinLoss { get; }

        public string UserName { get; }
    }
}