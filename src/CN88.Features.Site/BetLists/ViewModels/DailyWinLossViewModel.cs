namespace CN88.Features.Site.BetLists.ViewModels
{
    using System.Collections.Generic;
    using Features.BetLists.Models;
    using Features.Shared.Models;
    using Requests;

    public class DailyWinLossViewModel
    {
        public DailyWinLossViewModel(DailyWinLossQuery message, IList<DailyWinLossModel> winLoss)
        {
            CustLevel = message.CustLevel;

            NoOfColumns = CustLevel.IsSuperLevel() ? 11 : CustLevel.IsMasterLevel() ? 9 : 8;
            WinLoss = winLoss;
        }

        public UserLevel CustLevel { get; }

        public int NoOfColumns { get; }

        public IList<DailyWinLossModel> WinLoss { get; }
    }
}