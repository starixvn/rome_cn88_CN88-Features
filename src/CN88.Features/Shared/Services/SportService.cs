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

    public interface SportService
    {
        Task<IEnumerable<Sport>> GetSports();
    }

    public class SportServiceImpl : SportService
    {
        private const string SportsCacheKey = "CN88.Features.Shared.Services.SportServiceImpl.Sports";

        private readonly IDynamicRepository dynamicRepository;
        private readonly Func<int, string> getSportDisplayName;

        public SportServiceImpl(
            IDynamicRepository dynamicRepository,
            Func<int, string> getSportDisplayName)
        {
            this.dynamicRepository = dynamicRepository;
            this.getSportDisplayName = getSportDisplayName;
        }

        public async Task<IEnumerable<Sport>> GetSports()
            => await ConfiguredCacheService.GetOrSetAsync(
                SportsCacheKey,
                () =>
                {
                    var sports = dynamicRepository.Fetch<Sport>(new GetSportsCriteria());

                    return sports.Select(sport => new Sport(sport.Id, sport.Name, getSportDisplayName));
                },
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));
    }
}