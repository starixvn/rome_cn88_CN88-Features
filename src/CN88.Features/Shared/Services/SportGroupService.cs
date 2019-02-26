namespace CN88.Features.Shared.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria;
    using DataAccesses;
    using Fanex.Caching;
    using Models;

    public interface SportGroupService
    {
        IEnumerable<SportGroup> GetSportGroups();
    }

    public class SportGroupServiceImpl : SportGroupService
    {
        private const string SportGroupsCacheKey = "CN88.Features.Shared.Services.SportServiceImpl.SportGroups";

        private readonly SportGroupDataAccess sportGroupDataAccess;
        private readonly Func<int, string> getSportDisplayName;

        public SportGroupServiceImpl(
            SportGroupDataAccess sportGroupDataAccess,
            Func<int, string> getSportDisplayName)
        {
            this.sportGroupDataAccess = sportGroupDataAccess;
            this.getSportDisplayName = getSportDisplayName;
        }

        public IEnumerable<SportGroup> GetSportGroups()
            => ConfiguredCacheService.GetOrSet(
                SportGroupsCacheKey,
                () =>
                {
                    var sportGroups = sportGroupDataAccess.GetSportGroups(new GetSportGroupsCriteria());

                    return sportGroups.Select(group => new SportGroup(group.Id, group.DisplaySportIds, getSportDisplayName));
                },
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));
    }
}