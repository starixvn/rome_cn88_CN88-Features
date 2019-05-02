namespace CN88.Features.BetLists
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Criteria.WagerMonitoring;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Helpers;
    using Models;
    using Models.WagerMonitoring;
    using Params;
    using Shared.Configurations;
    using Shared.DataAccesses;
    using Shared.Extensions;
    using Shared.Models;
    using Shared.Services;

    public interface WagerMonitoringService
    {
        Task<IEnumerable<SportGroup>> GetSportGroups();

        Task<IEnumerable<League>> GetLeagues(int sportGroup);

        Task<IEnumerable<MatchInfo>> GetMatches(int sportGroup);

        Task<IEnumerable<MonitorBetTypeGroup>> GetBetTypeGroupsBySport(int sportGroup);

        Task<IEnumerable<TicketDto>> GetMonitorBets(WagerMonitoringParam param);
    }

    public class WagerMonitoringServiceImpl : WagerMonitoringService
    {
        private const string MonitorSportGroupsCacheKey = "CN88.Features.BetLists.WagerMonitoringServiceImpl.MonitorSportGroups";
        private const string MonitorBetTypeGroupsCacheKey = "CN88.Features.BetLists.WagerMonitoringServiceImpl.MonitorBetTypeGroups";
        private const string MatchesCacheKey = "CN88.Features.BetLists.WagerMonitoringServiceImpl.Matches";
        private const string OtherMatchesCacheKey = "CN88.Features.BetLists.WagerMonitoringServiceImpl.OtherMatches";
        private const string OutrightMatchesCacheKey = "CN88.Features.BetLists.WagerMonitoringServiceImpl.OutrightMatches";
        private const string MinTransIdCachesKey = "CN88.Features.BetLists.WagerMonitoringServiceImpl.MinTransId";

        private readonly IDynamicRepository dynamicRepository;
        private readonly CacheRefService cacheRefService;
        private readonly SportGroupDataAccess sportGroupDataAccess;

        public WagerMonitoringServiceImpl(
            IDynamicRepository dynamicRepository,
            CacheRefService cacheRefService,
            SportGroupDataAccess sportGroupDataAccess)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheRefService = cacheRefService;
            this.sportGroupDataAccess = sportGroupDataAccess;
        }

        public async Task<IEnumerable<SportGroup>> GetSportGroups()
            => await ConfiguredCacheService.GetOrSetAsync(
                MonitorSportGroupsCacheKey,
                () => sportGroupDataAccess.GetSportGroups(new GetMonitorSportGroupCriteria(), cacheRefService.GetSportName),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));

        public async Task<IEnumerable<League>> GetLeagues(int sportGroup)
        {
            var matches = await GetMatches(sportGroup);

            return matches.GroupBy(e => e.LeagueId)
                .Select(g => new League(g.Key, cacheRefService.GetLeagueName(g.Key)))
                .OrderBy(l => l.Name);
        }

        public async Task<IEnumerable<MatchInfo>> GetMatches(int sportGroup)
        {
            var otherEvents = await GetOtherMatches();
            var outrightEvents = await GetOutrightMatches();

            var events = new List<MatchInfo>(otherEvents);
            events.AddRange(outrightEvents);

            return sportGroup == 0
                ? events
                : events.Where(e => e.SportTypeGroup == sportGroup);
        }

        public async Task<IEnumerable<MonitorBetTypeGroup>> GetBetTypeGroupsBySport(int sportGroup)
            => (await GetBetTypeGroups()).Where(group => group.SportTypeGroup == sportGroup);

        public async Task<IEnumerable<TicketDto>> GetMonitorBets(WagerMonitoringParam param)
        {
            var betTypeGroupIds = param.DisplayBetTypeGroupIds.ToListInt();
            var betTypeGroups = await GetBetTypeGroupsBySport(param.SportGroupId);

            var criteria = new WagerMonitoringCriteria(
                param.CustId,
                param.CustLevelId,
                param.StakeConditons,
                param.Market,
                param.LeagueIds,
                param.MatchIds,
                param.DisplayBetTypeGroupIds,
                AppSettings.WagerMonitoringPageSize,
                ConfiguredCacheService.Get<long>($"{MinTransIdCachesKey}.{param.CustId}"));

            var tickets = await dynamicRepository.FetchAsync<TicketDto>(criteria);
            tickets = DataAccesses.BetListsData.GetMonitorBets(param);

            ConfiguredCacheService.Set($"{MinTransIdCachesKey}.{param.CustId}", criteria.MinTransId);

            if (tickets.Any())
            {
                var outrightMatches = await GetOutrightMatches();
                var otherMatches = await GetOtherMatches();

                foreach (var ticket in tickets)
                {
                    var matches = BetTypeHelper.IsOutRight(ticket.BetTypeId) ? outrightMatches : otherMatches;
                    var matchInfo = await GetMatchInfo(ticket.MatchId, matches);

                    AssignMatchInfoToTicket(ticket, matchInfo);
                }
            }

            return tickets;
        }

        private async Task<IEnumerable<MatchInfo>> GetOtherMatches()
            => await ConfiguredCacheService.GetOrSetAsync(
                OtherMatchesCacheKey,
                () =>
                {
                    var matches = dynamicRepository.Fetch<MatchInfo>(new GetOtherMatchCriteria());
                    foreach (var match in matches)
                    {
                        match.MatchName = $"{cacheRefService.GetTeamName(match.HomeId)} v {cacheRefService.GetTeamName(match.AwayId)}";
                    }

                    return matches;
                },
                new CacheItemOptions().SetAbsoluteExpiration(
                    TimeSpan.FromMinutes(AppSettings.WagerMonitoringExpirationCacheInMinutes)));

        private async Task<IEnumerable<MatchInfo>> GetOutrightMatches()
            => await ConfiguredCacheService.GetOrSetAsync(
                OutrightMatchesCacheKey,
                () =>
                {
                    var matches = dynamicRepository.Fetch<MatchInfo>(new GetOutrightMatchCriteria());
                    foreach (var match in matches)
                    {
                        match.MatchName = $"{cacheRefService.GetTeamName(match.HomeId)}";
                    }

                    return matches;
                },
                new CacheItemOptions().SetAbsoluteExpiration(
                    TimeSpan.FromMinutes(AppSettings.WagerMonitoringExpirationCacheInMinutes)));

        private async Task<MatchInfo> GetMatchInfo(int matchId, IEnumerable<MatchInfo> matches)
        {
            var cachedKey = $"{MatchesCacheKey}_{matchId}";

            var matchInfo = matches.FirstOrDefault(match => match.MatchId == matchId)
                ?? await ConfiguredCacheService.GetAsync<MatchInfo>(cachedKey);

            if (matchInfo == null && !IsDummyMatch(matchId))
            {
                matchInfo = await dynamicRepository.GetAsync<MatchInfo>(new GetMatchByIdCriteria(matchId));

                if (matchInfo != null)
                {
                    await ConfiguredCacheService.SetAsync(
                        cachedKey,
                        matchInfo,
                        new CacheItemOptions().SetAbsoluteExpiration(
                            TimeSpan.FromMinutes(AppSettings.WagerMonitoringExpirationCacheInMinutes)));
                }
            }

            return matchInfo;
        }

        private async Task<IEnumerable<MonitorBetTypeGroup>> GetBetTypeGroups()
            => await ConfiguredCacheService.GetOrSetAsync(
                MonitorBetTypeGroupsCacheKey,
                () =>
                {
                    var betTypeGroups = dynamicRepository.Fetch<MonitorBetTypeGroup>(new GetMonitorBetTypeGroupCriteria());
                    foreach (var betTypeGroup in betTypeGroups)
                    {
                        betTypeGroup.DisplayName = cacheRefService.GetBetTypeName(betTypeGroup.DisplayBetTypeGroup);
                    }

                    return betTypeGroups;
                },
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));

        private static bool IsDummyMatch(int matchId) => matchId <= 999;

        private static void AssignMatchInfoToTicket(TicketDto ticket, MatchInfo matchInfo)
        {
            if (matchInfo == null)
            {
                return;
            }

            ticket.MatchCode = matchInfo.MatchCode;
            ticket.SportTypeId = matchInfo.SportType;
            ticket.LeagueId = matchInfo.LeagueId;
            ticket.HomeId = matchInfo.HomeId;
            ticket.AwayId = matchInfo.AwayId;
            ticket.Race = matchInfo.Race;
            ticket.ShowTime = matchInfo.ShowTime;
            ticket.IsNeutral = matchInfo.IsNeutral;
            ticket.EventStatus = matchInfo.BetStatus;
            ticket.SettlementStatus = matchInfo.SettlementStatus;
        }
    }
}