namespace CN88.Features.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Criteria;
    using DataAccesses;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;
    using Params;
    using Shared.Services;

    public interface ReportService
    {
        Task<IEnumerable<WinLossModel>> GetWinLoss(ReportParam param);

        Task<IEnumerable<AccountingPeriodGroup>> GetAccountingPeriodGroups();

        Task<AccountingPeriod> GetCurrentPeriod();
    }

    public class ReportServiceImpl : ReportService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly ReportDateService reportDateService;
        private readonly ReportDataAccess reportDataAccess;

        public ReportServiceImpl(
            IDynamicRepository dynamicRepository,
            ReportDateService reportDateService,
            ReportDataAccess reportDataAccess)
        {
            this.dynamicRepository = dynamicRepository;
            this.reportDateService = reportDateService;
            this.reportDataAccess = reportDataAccess;
        }

        public async Task<IEnumerable<WinLossModel>> GetWinLoss(ReportParam param)
            => await reportDataAccess.GetWinLoss(
                new GetWinLossReportCriteria(
                    param.CustId,
                    param.IsOwner,
                    param.FromDate,
                    param.ToDate,
                    await reportDateService.GetBalanceUpDay(),
                    await reportDateService.GetMoveBetsDay(),
                    param.SportTypeId,
                    param.BetTypeId,
                    param.IsLive,
                    param.ReportType,
                    param.ReportLevel));

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
            => (await GetAccountingPeriods()).FirstOrDefault(p => p.IsCurrentPeriod);
    }
}