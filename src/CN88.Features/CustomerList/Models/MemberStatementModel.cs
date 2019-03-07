namespace CN88.Features.CustomerList.Models
{
    using System;
    using CN88.Features.Shared.Extensions;
    using CN88.Features.Extensions;
    public class MemberStatementModel
    {
        public MemberStatementModel()
        {

        }

        public DateTime WinLostDate { get; }

        public string WinLostDateStr => WinLostDate.ToLongDay();

        public decimal? Stake { get; }

        public string StakeStr => Stake.HasValue ? Stake.Value.FormatNumber() : "-";

        public decimal? ValidStake { get; }

        public string ValidStakeStr => ValidStake.HasValue ? ValidStake.Value.FormatNumber() : "-";

        public decimal? WinLoss { get; }

        public string WinLossStr => WinLoss.HasValue ? WinLoss.Value.FormatNumber() : "-";

        public bool HasTicket => Stake.HasValue || ValidStake.HasValue || WinLoss.HasValue;
    }
}
