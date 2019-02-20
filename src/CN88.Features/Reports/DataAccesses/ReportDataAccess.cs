namespace CN88.Features.Reports.DataAccesses
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Criteria;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Models;
    using Shared.Models;

    public interface ReportDataAccess
    {
        Task<IEnumerable<WinLossModel>> GetWinLoss(GetWinLossReportCriteria criteria);
    }

    public class ReportDataAccessImpl : ReportDataAccess
    {
        public async Task<IEnumerable<WinLossModel>> GetWinLoss(GetWinLossReportCriteria criteria)
        {
            var winlossData = new List<WinLossModel>();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = await db.ExecuteReaderAsync(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            winlossData.Add(ReadWinLoss(reader, criteria.ReportLevel));
                        }
                    }
                }
            }

            return winlossData;
        }

        private static WinLossModel ReadWinLoss(IDataReader reader, UserLevel reportLevel)
            => new WinLossModel(
                custId: reader.ToInt("CustId"),
                username: reader.ToString("Username"),
                name: reader.ToString("Name"),
                wagers: reader.ToInt("Wagers"),
                stake: reader.ToDecimal("Stake"),
                validStake: reader.ToDecimal("ValidStake"),
                memberIncome: reader.ToDecimal("MemberIncome"),
                agentCurrency: reportLevel.IsAgentLevel() ? reader.ToDecimal("AgentCurrency") : (decimal?)null,
                headlineNetWinLoss: NetWinLoss.ReadHeadlineNetWinLoss(reader, reportLevel),
                superNetWinLoss: NetWinLoss.ReadSuperNetWinLoss(reader, reportLevel),
                masterNetWinLoss: NetWinLoss.ReadMasterNetWinLoss(reader, reportLevel),
                agentNetWinLoss: NetWinLoss.ReadAgentNetWinLoss(reader, reportLevel));
    }
}