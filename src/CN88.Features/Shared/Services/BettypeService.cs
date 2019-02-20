namespace CN88.Features.Shared.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Criteria;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models;

    public interface BettypeService
    {
        Task<IEnumerable<BetType>> GetBetTypesBySport(int sportGroupId = 0);
    }

    public class BettypeServiceImpl : BettypeService
    {
        private const string BetTypesCacheKey = "CN88.Features.Shared.Services.Bettypes";

        private readonly IDynamicRepository dynamicRepository;
        private readonly Func<int, string> getBetTypeDisplayName;

        public BettypeServiceImpl(
            IDynamicRepository dynamicRepository,
            Func<int, string> getBetTypeDisplayName)
        {
            this.dynamicRepository = dynamicRepository;
            this.getBetTypeDisplayName = getBetTypeDisplayName;
        }

        public async Task<IEnumerable<BetType>> GetBetTypesBySport(int sportGroupId = 0)
            => await ConfiguredCacheService.GetOrSetAsync(
                $"{BetTypesCacheKey}.{sportGroupId}",
                () =>
                {
                    var betTypes = dynamicRepository.Fetch<BetType>(new GetBetTypeBySportCriteria(sportGroupId));

                    return betTypes.Select(betType => new BetType(betType.Id, betType.Name, getBetTypeDisplayName));
                },
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));
    }
}