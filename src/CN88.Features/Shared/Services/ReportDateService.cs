using System;
using System.Threading.Tasks;
using CN88.Features.Shared.Criteria;
using CN88.Features.Shared.Models;
using Fanex.Caching;
using Fanex.Data.Repository;

namespace CN88.Features.Shared.Services
{
    public interface ReportDateService
    {
        Task<BalanceUpDayAndMoveBetsDay> GetBalanceUpDayAndMoveBetsDay();

        Task<DateTime> GetBalanceUpDay();

        Task<DateTime> GetMoveBetsDay();
    }

    public class ReportDateServiceImpl : ReportDateService
    {
        private const string BalanceUpDayAndMoveBetsDayCacheKey = "CN88.Features.Shared.Services.ReportDateServiceImpl.BalanceUpDayAndMoveBetsDay";

        private readonly IDynamicRepository dynamicRepository;

        public ReportDateServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public static ReportDateService Instance { get; set; }

        public async Task<DateTime> GetBalanceUpDay() => (await GetBalanceUpDayAndMoveBetsDay()).BalanceUpDay;

        public async Task<DateTime> GetMoveBetsDay() => (await GetBalanceUpDayAndMoveBetsDay()).MoveBetsDay;

        public async Task<BalanceUpDayAndMoveBetsDay> GetBalanceUpDayAndMoveBetsDay()
            => await ConfiguredCacheService.GetOrSetAsync(
                BalanceUpDayAndMoveBetsDayCacheKey,
                () => dynamicRepository.Get<BalanceUpDayAndMoveBetsDay>(new GetBalanceUpDayAndMoveBetsDayCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
    }
}