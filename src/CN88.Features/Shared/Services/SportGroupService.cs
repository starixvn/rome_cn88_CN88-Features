namespace CN88.Features.Shared.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CN88.Features.Shared.Constants;
    using Criteria;
    using DataAccesses;
    using Fanex.Caching;
    using Models;

    public interface SportGroupService
    {
        IEnumerable<SportGroup> GetSportGroups();
        IEnumerable<int> GetSportGroupIdsByType(int sportGroupType);
    }

    public class SportGroupServiceImpl : SportGroupService
    {
        private const string SportGroupsCacheKey = "CN88.Features.Shared.Services.SportServiceImpl.SportGroups";

        private readonly SportGroupDataAccess sportGroupDataAccess;
        private readonly CacheRefService cacheRefService;

        public SportGroupServiceImpl(
            SportGroupDataAccess sportGroupDataAccess,
            CacheRefService cacheRefService)
        {
            this.sportGroupDataAccess = sportGroupDataAccess;
            this.cacheRefService = cacheRefService;
        }

        public IEnumerable<SportGroup> GetSportGroups()
            => ConfiguredCacheService.GetOrSet(
                SportGroupsCacheKey,
                () => sportGroupDataAccess.GetSportGroups(new GetSportGroupsCriteria(), cacheRefService.GetSportName),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));

        public IEnumerable<int> GetSportGroupIdsByType(int sportGroupType)
        {
            var sports = GetSportGroups();
            if (sportGroupType == SportGroupTypeConstants.Sportsbook
                || sportGroupType == SportGroupTypeConstants.NonSports)
            {
                sports = sports.Where(sport => sport.ProductId == sportGroupType);
            }

            return sports.Select(i => i.Id);
        }
    }
}