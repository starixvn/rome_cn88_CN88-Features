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
        IEnumerable<BetType> GetBetTypeGroupsBySport(int sportGroupId = 0);
    }

    public class BettypeGroupServiceImpl : BettypeGroupService
    {
        private const string BetTypesCacheKey = "CN88.Features.Shared.Services.Bettypes";

        private readonly IDynamicRepository dynamicRepository;
        private readonly Func<int, string> getBetTypeDisplayName;

        public BettypeGroupServiceImpl(
            IDynamicRepository dynamicRepository,
            Func<int, string> getBetTypeDisplayName)
        {
            this.dynamicRepository = dynamicRepository;
            this.getBetTypeDisplayName = getBetTypeDisplayName;
        }

        public IEnumerable<BetType> GetBetTypeGroupsBySport(int sportGroupId = 0)
            => ConfiguredCacheService.GetOrSet(
                $"{BetTypesCacheKey}.{sportGroupId}",
                () =>
                {
                    var betTypes = dynamicRepository.Fetch<BetType>(new GetBetTypeGroupBySportCriteria());

                    return sportGroupId != 0 ? 
                        betTypes.Select(betType => new BetType(betType.SportTypeGroup, betType.Id, betType.Name, getBetTypeDisplayName)).Where(i => i.SportTypeGroup == sportGroupId) : 
                        betTypes.Select(betType => new BetType(betType.SportTypeGroup, betType.Id, betType.Name, getBetTypeDisplayName));
                },
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));
    }
}