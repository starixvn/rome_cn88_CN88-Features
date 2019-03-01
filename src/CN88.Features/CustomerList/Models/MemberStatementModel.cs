namespace CN88.Features.CustomerList.Models
{
    using System;

    public class MemberStatementModel
    {
        public MemberStatementModel()
        {

        }

        public DateTime WinLostDate { get; }

        public decimal? Stake { get; }

        public decimal? ValidStake { get; }

        public decimal? WinLoss { get; }
    }
}
