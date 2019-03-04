namespace CN88.Features.Site.BetLists.ViewModels
{
    using System.Linq;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Features.BetLists.Models;

    public class BetListTotal
    {
        public BetListTotal(BetListData betListData, string[] excludedStatus = null)
        {
            var exStatus = excludedStatus ?? new string[] { };

            var tickets = betListData.Tickets.Where(ticket => NotInExcludedStatus(ticket, exStatus));

            TotalWinLoss = tickets.Sum(ticket => ticket.Winlost);
            TotalStake = tickets.Sum(ticket => ticket.Stake);
            TotalCommission = tickets.Sum(ticket => ticket.PlayerComm);

            TotalStr = ConvertByBetType.Winloss(TotalWinLoss + TotalCommission);
        }

        public decimal TotalWinLoss { get; }

        public decimal TotalStake { get; }

        public decimal TotalCommission { get; }

        public string TotalWinLossStr => ConvertByBetType.Winloss(TotalWinLoss);

        public string TotalStakeStr => ConvertByBetType.Stake(TotalStake);

        public string TotalStr { get; }

        private static bool NotInExcludedStatus(ITicket ticket, string[] excludedStatus)
            => !excludedStatus.Contains(ticket.Status.ToLowerInvariant());
    }
}