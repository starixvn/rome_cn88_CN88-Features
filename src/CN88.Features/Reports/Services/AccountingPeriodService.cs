namespace CN88.Features.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;

    public interface AccountingPeriodService
    {
        IEnumerable<AccountingPeriodGroup> GetAccountingPeriodGroups();

        AccountingPeriod GetCurrentPeriod();

        AccountingPeriod GetLastPeriod();
    }

    public class AccountingPeriodServiceImpl : AccountingPeriodService
    {
        private const string AccountingPeriodsCacheKey = "CN88.Features.Reports.Services.AccountingPeriodServiceImpl.AccountingPeriods";

        private readonly IDynamicRepository dynamicRepository;

        public AccountingPeriodServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public IEnumerable<AccountingPeriodGroup> GetAccountingPeriodGroups()
        {
            var periods = GetAccountingPeriods();
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

        public AccountingPeriod GetCurrentPeriod()
            => GetAccountingPeriods().FirstOrDefault(p => p.IsCurrentPeriod);

        public AccountingPeriod GetLastPeriod()
        {
            var date = GetCurrentPeriod().FromDate.AddDays(-2);

            return GetAccountingPeriods().FirstOrDefault(p => p.FromDate <= date && date <= p.ToDate);
        }

        private IEnumerable<AccountingPeriod> GetAccountingPeriods()
            => ConfiguredCacheService.GetOrSet(
                AccountingPeriodsCacheKey,
                () => dynamicRepository.Fetch<AccountingPeriod>(new GetAccountingPeriodsCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));
    }
}