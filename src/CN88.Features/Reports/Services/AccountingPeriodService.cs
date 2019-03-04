namespace CN88.Features.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;
    using Shared.Configurations;

    public interface AccountingPeriodService
    {
        AccountingPeriod GetCurrentPeriod();

        AccountingPeriod GetLastPeriod();

        IEnumerable<AccountingPeriod> GetOldAccountingPeriods();

        IEnumerable<AccountingPeriod> GetAccountingPeriods();
    }

    public class AccountingPeriodServiceImpl : AccountingPeriodService
    {
        private const string AccountingPeriodsCacheKey = "CN88.Features.Reports.Services.AccountingPeriodServiceImpl.AccountingPeriods";

        private readonly IDynamicRepository dynamicRepository;

        public AccountingPeriodServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public AccountingPeriod GetCurrentPeriod()
            => GetAccountingPeriods().FirstOrDefault(p => p.IsCurrentPeriod);

        public AccountingPeriod GetLastPeriod()
        {
            var date = GetCurrentPeriod().FromDate.AddDays(-2);

            return GetAccountingPeriods().FirstOrDefault(p => p.FromDate <= date && date <= p.ToDate);
        }

        public IEnumerable<AccountingPeriod> GetOldAccountingPeriods()
        {
            var periods = GetAccountingPeriods().ToList();

            var currentPeriod = periods.FirstOrDefault(p => p.IsCurrentPeriod);
            var currentIndex = periods.IndexOf(currentPeriod);

            return periods
                .Where(p => periods.IndexOf(p) < (currentIndex - 1))
                .OrderByDescending(p => p.FromDate)
                .Take(AppSettings.NumberOfPeriod);
        }

        public IEnumerable<AccountingPeriod> GetAccountingPeriods()
            => ConfiguredCacheService.GetOrSet(
                AccountingPeriodsCacheKey,
                () => dynamicRepository.Fetch<AccountingPeriod>(new GetAccountingPeriodsCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));
    }
}