namespace CN88.Features.Reports.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CN88.Features.Reports.Criteria;
    using CN88.Features.Reports.Models;
    using CN88.Features.Reports.Params;
    using Fanex.Data.Repository;

    public interface ReportService
    {
        Task<IEnumerable<ReportModel>> GetWagerReport(ReportParam param);

        Task<IEnumerable<ReportModel>> GetLegacyreport(ReportParam param);

        Task<IEnumerable<AccountingGroup>> GetAccountingPeriods();
    }

    public class ReportServiceImpl : ReportService
    {
        private readonly IDynamicRepository dynamicRepository;

        public ReportServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<ReportModel>> GetWagerReport(ReportParam param)
            => await dynamicRepository.FetchAsync<ReportModel>(
                new WagerReportCriteria(
                    param.ReportType,
                    param.FromDate,
                    param.ToDate,
                    param.SportTypeId,
                    param.BetTypeId));

        public async Task<IEnumerable<ReportModel>> GetLegacyreport(ReportParam param)
            => await dynamicRepository.FetchAsync<ReportModel>(
                new LegacyReportCriteria(
                    param.ReportType,
                    param.FromDate,
                    param.ToDate,
                    param.SportTypeId,
                    param.BetTypeId));

        public async Task<IEnumerable<AccountingGroup>> GetAccountingPeriods()
        {
            var periods = await dynamicRepository.FetchAsync<AccountingPeriod>(new GetAccountingPeriodsCriteria());
            var groups = periods.GroupBy(p => p.Year.ToString()).OrderByDescending(g => g.Key);
            var result = new List<AccountingGroup>();
            foreach (var group in groups)
            {
                var ps = new List<AccountingPeriod>();

                foreach (var p in group)
                {
                    ps.Add(p);
                }

                result.Add(new AccountingGroup(group.Key, ps.OrderBy(p => p.PeriodId).ToList()));
            }

            return result;
        }
    }
}