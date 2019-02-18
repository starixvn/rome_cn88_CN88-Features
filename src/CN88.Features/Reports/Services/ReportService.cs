namespace CN88.Features.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Criteria;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;
    using Params;
    using Shared.Utils;

    public interface ReportService
    {
        Task<IEnumerable<ReportModel>> GetReportData(ReportParam param);

        Task<IEnumerable<AccountingPeriodGroup>> GetAccountingPeriodGroups();

        Task<AccountingPeriod> GetCurrentPeriod();
    }

    public class ReportServiceImpl : ReportService
    {
        private readonly IDynamicRepository dynamicRepository;

        public ReportServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<ReportModel>> GetReportData(ReportParam param)
            => await dynamicRepository.FetchAsync<ReportModel>(
                new ReportCriteria(
                    param.ReportType,
                    param.FromDate,
                    param.ToDate,
                    param.SportTypeId,
                    param.BetTypeId));

        public async Task<IEnumerable<AccountingPeriodGroup>> GetAccountingPeriodGroups()
        {
            var periods = await GetAccountingPeriods();
            var groups = periods
                .OrderBy(p => p.PeriodId)
                .GroupBy(p => p.PeriodId.ToString());
            var result = new List<AccountingPeriodGroup>();

            foreach (var group in groups)
            {
                var ps = new List<AccountingPeriod>();

                foreach (var p in group)
                {
                    ps.Add(p);
                }

                result.Add(new AccountingPeriodGroup(group.Key, ps.OrderByDescending(p => p.Year).ToList()));
            }

            return result;
        }

        private async Task<IEnumerable<AccountingPeriod>> GetAccountingPeriods()
            => await ConfiguredCacheService.GetOrSetAsync(
                "CN88.Features.Reports.Services.ReportServiceImpl.AccountingPeriods",
                () => dynamicRepository.Fetch<AccountingPeriod>(new GetAccountingPeriodsCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));

        public async Task<AccountingPeriod> GetCurrentPeriod()
            => (await GetAccountingPeriods()).FirstOrDefault(
                    p => p.FromDate <= SystemTime.Today
                        && SystemTime.Today >= p.ToDate);
    }
}