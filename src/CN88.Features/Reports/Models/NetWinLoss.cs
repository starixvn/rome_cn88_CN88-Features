namespace CN88.Features.Reports.Models
{
    using System.Data;
    using Fanex.Common.Extensions;
    using Shared.Models;

    public class NetWinLoss
    {
        public NetWinLoss(
            decimal winloss,
            decimal result)
        {
            WinLoss = winloss;
            Result = result;
        }

        public NetWinLoss(
            decimal income,
            decimal result,
            decimal? stock)
        {
            Income = income;
            Result = result;
            Stock = stock;
        }

        public NetWinLoss(
            decimal? winloss,
            decimal? poss,
            decimal? income,
            decimal? result,
            decimal? stock)
        {
            WinLoss = winloss;
            Poss = poss;
            Income = income;
            Result = result;
            Stock = stock;
        }

        public decimal? WinLoss { get; private set; }

        public decimal? Poss { get; private set; }

        public decimal? Income { get; private set; }

        public decimal? Result { get; private set; }

        public decimal? Stock { get; private set; }

        public static NetWinLoss Add(NetWinLoss winlossA, NetWinLoss winlossB)
        {
            if (winlossA == null || winlossB == null)
            {
                return winlossA ?? winlossB;
            }

            return new NetWinLoss(
                winloss: winlossA?.WinLoss + winlossB?.WinLoss,
                poss: null,
                income: winlossA?.Income + winlossB?.Income,
                result: winlossA?.Result + winlossB?.Result,
                stock: winlossA?.Stock + winlossB?.Stock);
        }

        public static NetWinLoss ReadHeadlineNetWinLoss(IDataReader reader, UserLevel reportLevel)
            => reportLevel.IsSuperLevel()
                ? new NetWinLoss(
                    winloss: null,
                    poss: reader.ToDecimal("HeadlinePoss"),
                    income: reader.ToDecimal("HeadlineIncome"),
                    result: reader.ToDecimal("HeadlineResult"),
                    stock: reader.ToDecimal("HeadlineStock"))
                : null;

        public static NetWinLoss ReadSuperNetWinLoss(IDataReader reader, UserLevel reportLevel)
            => reportLevel.IsMasterOrSuperLevel()
                ? new NetWinLoss(
                    winloss: reader.ToDecimal("Super"),
                    poss: reportLevel.IsMasterLevel() ? reader.ToDecimal("SuperPoss") : (decimal?)null,
                    income: reader.ToDecimal("SuperIncome"),
                    result: reader.ToDecimal("SuperResult"),
                    stock: reader.ToDecimal("SuperStock")) :
                null;

        public static NetWinLoss ReadMasterNetWinLoss(IDataReader reader, UserLevel reportLevel)
        {
            if (reportLevel.IsSuperLevel())
            {
                return new NetWinLoss(
                    winloss: reader.ToDecimal("Master"),
                    result: reader.ToDecimal("MasterResult"));
            }

            if (reportLevel.IsMasterLevel())
            {
                return new NetWinLoss(
                    income: reader.ToDecimal("MasterIncome"),
                    result: reader.ToDecimal("MasterResult"),
                    stock: reader.ToDecimal("MasterStock"));
            }

            if (reportLevel.IsAgentLevel())
            {
                return new NetWinLoss(
                    income: reader.ToDecimal("MasterIncome"),
                    result: reader.ToDecimal("MasterResult"),
                    stock: null);
            }

            return null;
        }

        public static NetWinLoss ReadAgentNetWinLoss(IDataReader reader, UserLevel reportLevel)
        {
            if (reportLevel.IsAgentLevel())
            {
                return new NetWinLoss(
                    winloss: reader.ToDecimal("Agent"),
                    poss: null,
                    income: reader.ToDecimal("AgentIncome"),
                    result: reader.ToDecimal("AgentResult"),
                    stock: reader.ToDecimal("AgentStock"));
            }

            if (reportLevel.IsMemberLevel())
            {
                return new NetWinLoss(
                    income: reader.ToDecimal("AgentIncome"),
                    result: reader.ToDecimal("AgentResult"),
                    stock: null);
            }

            return new NetWinLoss(
                winloss: reader.ToDecimal("Agent"),
                result: reader.ToDecimal("AgentResult"));
        }
    }
}