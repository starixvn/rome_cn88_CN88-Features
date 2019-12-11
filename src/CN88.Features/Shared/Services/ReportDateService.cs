using System;
using CN88.Features.Shared.Criteria;
using CN88.Features.Shared.Models;
using Fanex.Caching;
using Fanex.Data.Repository;

namespace CN88.Features.Shared.Services
{
    public interface ReportDateService
    {
        BalanceUpDayAndMoveBetsDay GetBalanceUpDayAndMoveBetsDay();

        DateTime GetBalanceUpDay();

        DateTime GetMoveBetsDay();
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

        public DateTime GetBalanceUpDay() => GetBalanceUpDayAndMoveBetsDay().BalanceUpDay;

        public DateTime GetMoveBetsDay() => GetBalanceUpDayAndMoveBetsDay().MoveBetsDay;

        public BalanceUpDayAndMoveBetsDay GetBalanceUpDayAndMoveBetsDay()
            => ConfiguredCacheService.GetOrSet(
                BalanceUpDayAndMoveBetsDayCacheKey,
                () => dynamicRepository.Get<BalanceUpDayAndMoveBetsDay>(new GetBalanceUpDayAndMoveBetsDayCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
    }
}