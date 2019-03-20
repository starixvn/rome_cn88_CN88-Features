namespace CN88.Features.Site.BetLists.ViewModels
{
    using System.Linq;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Features.BetLists.Models;
    using Helpers;

    public class BetListTotal
    {
        public BetListTotal(BetListData betListData, int divisor, string[] excludedStatus = null)
        {
            var exStatus = excludedStatus ?? new string[] { };

            var tickets = betListData.Tickets.Where(ticket => NotInExcludedStatus(ticket, exStatus));

            TotalWinLoss = tickets.Sum(ticket => ticket.Winlost) / divisor;
            TotalStake = tickets.Sum(ticket => ticket.Stake) / divisor;
            TotalCommission = tickets.Sum(ticket => ticket.PlayerComm);

            WinLossWithCommission = TotalWinLoss + TotalCommission / divisor;
        }

        public decimal TotalWinLoss { get; }

        public decimal TotalStake { get; }

        public decimal TotalCommission { get; }

        public decimal WinLossWithCommission { get; }

        public string TotalWinLossStr => ConvertByBetType.Winloss(TotalWinLoss);

        public string TotalStakeStr => BetListHelpers.Stake(TotalStake);

        public string TotalStr => ConvertByBetType.Winloss(WinLossWithCommission);

        private static bool NotInExcludedStatus(ITicket ticket, string[] excludedStatus)
            => !excludedStatus.Contains(ticket.Status.ToLowerInvariant());
    }
}