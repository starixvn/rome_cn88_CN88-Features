namespace CN88.Features.CustomerList.Models
{
    using System;
    using System.Resources;
    using Extensions;
    using Fanex.BetList.Core.Utils;
    using Resources;

    public class MemberStatementModel
    {
        private readonly ResourceManager resourceManager;

        public MemberStatementModel()
        {
            resourceManager = new ResourceManager(typeof(CustomerListResources));
        }

        public DateTime WinLostDate { get; }

        public string WinLostDateStr
            => $"{WinLostDate.ToShortDateFormat()} {resourceManager.GetString(WinLostDate.ToString("dddd"))}";

        public decimal? Stake { get; }

        public string StakeStr => Stake.HasValue ? Stake.Value.ToNumberWithoutDecimal() : "-";

        public decimal? ValidStake { get; }

        public string ValidStakeStr => ValidStake.HasValue ? ValidStake.Value.ToNumberWithoutDecimal() : "-";

        public decimal? WinLoss { get; }

        public string WinLossStr => WinLoss.HasValue ? ConvertByBetType.Winloss(WinLoss.Value) : "-";

        public bool HasTicket => Stake.HasValue || ValidStake.HasValue || WinLoss.HasValue;
    }
}