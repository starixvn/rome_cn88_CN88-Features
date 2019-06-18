namespace CN88.Features.Shared.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;

    public interface BettypeGroupService
    {
        IEnumerable<BetTypeGroup> GetBetTypeGroups();
    }

    public class BettypeGroupServiceImpl : BettypeGroupService
    {
        private const string BetTypesCacheKey = "CN88.Features.Shared.Services.Bettypes";

        private readonly IDynamicRepository dynamicRepository;
        private readonly CacheRefService cacheRefService;

        public BettypeGroupServiceImpl(
            IDynamicRepository dynamicRepository,
            CacheRefService cacheRefService)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheRefService = cacheRefService;
        }

        public IEnumerable<BetTypeGroup> GetBetTypeGroups()
        {
            var bettypeGroups = QueryBetTypeGroups();
            var bettypeIds = bettypeGroups.Select(betType => betType.Id).Distinct();
            var bettypeNames = new Dictionary<int, string>();

            foreach (var id in bettypeIds)
            {
                bettypeNames.Add(id, cacheRefService.GetBetTypeName(id));
            }

            foreach (var betType in bettypeGroups)
            {
                betType.DisplayName = bettypeNames[betType.Id];
            }

            return bettypeGroups;
        }

        private IEnumerable<BetTypeGroup> QueryBetTypeGroups()
            => ConfiguredCacheService.GetOrSet(
                BetTypesCacheKey,
                () => dynamicRepository.Fetch<BetTypeGroup>(new GetBetTypeGroupBySportCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));
    }
}