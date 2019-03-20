namespace CN88.Features.CustomerList.Models
{
    using System;
    using CN88.Features.Extensions;
    using CN88.Features.Shared.Extensions;
    using Fanex.BetList.Core.Utils;

    public class MemberStatementModel
    {
        public MemberStatementModel()
        {

        }

        public DateTime WinLostDate { get; }

        public string WinLostDateStr => WinLostDate.ToLongDay();

        public decimal? Stake { get; }

        public string StakeStr => Stake.HasValue ? Stake.Value.ToNumberWithoutDecimal() : "-";

        public decimal? ValidStake { get; }

        public string ValidStakeStr => ValidStake.HasValue ? ValidStake.Value.ToNumberWithoutDecimal() : "-";

        public decimal? WinLoss { get; }

        public string WinLossStr => WinLoss.HasValue ? ConvertByBetType.Winloss(WinLoss.Value) : "-";

        public bool HasTicket => Stake.HasValue || ValidStake.HasValue || WinLoss.HasValue;
    }
}
