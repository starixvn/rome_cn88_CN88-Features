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
    using Resources;
    using Shared.Configurations;
    using Shared.Constants;
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
            var matches = await GetAllMatches(sportGroup);

            return matches.GroupBy(e => e.LeagueId)
                .Select(g => new League(g.Key, cacheRefService.GetLeagueName(g.Key)))
                .OrderBy(l => l.Name);
        }

        public async Task<IEnumerable<MatchInfo>> GetMatches(int sportGroup)
        {
            var matches = sportGroup == SportGroupConstants.AllSports
                ? await GetOtherMatches()
                : (await GetOtherMatches()).Where(m => m.SportTypeGroup == sportGroup);

            var matchIds = matches.Select(m => m.HomeId).Union(matches.Select(m => m.AwayId)).Distinct();
            var matchNames = new Dictionary<int, string>();

            foreach (var id in matchIds)
            {
                matchNames.Add(id, cacheRefService.GetTeamName(id));
            }

            foreach (var match in matches)
            {
                match.MatchName = $"{matchNames[match.HomeId]} v {matchNames[match.AwayId]}";
            }

            return matches;
        }

        private async Task<IEnumerable<MatchInfo>> GetAllMatches(int sportGroup)
        {
            var matches = (await GetOtherMatches()).ToList();

            if (sportGroup != SportGroupConstants.ParlaySportGroupId)
            {
                matches.AddRange(await GetOutrightMatches());
            }

            return sportGroup == 0
                ? matches
                : matches.Where(e => e.SportTypeGroup == sportGroup);
        }

        public async Task<IEnumerable<MonitorBetTypeGroup>> GetBetTypeGroupsBySport(int sportGroup)
        {
            var betTypeGroups = (await GetBetTypeGroups()).Where(group => group.SportTypeGroup == sportGroup);

            var displayBetTypeGroups = betTypeGroups.Select(g => g.DisplayBetTypeGroup).Distinct();
            var displayBetTypeNames = new Dictionary<int, string>();

            foreach (var id in displayBetTypeGroups)
            {
                displayBetTypeNames.Add(id, cacheRefService.GetBetTypeName(id));
            }

            foreach (var betTypeGroup in betTypeGroups)
            {
                betTypeGroup.DisplayName = IsOneXTwoMoneyline(betTypeGroup)
                    ? BetListsResources.OneXTwoMoneyline
                    : displayBetTypeNames[betTypeGroup.DisplayBetTypeGroup];
            }

            return betTypeGroups;
        }

        public async Task<IEnumerable<TicketDto>> GetMonitorBets(WagerMonitoringParam param)
        {
            var criteria = new WagerMonitoringCriteria(
                param.CustId,
                param.CustLevelId,
                param.SportGroupId,
                param.StakeConditons,
                param.Market,
                param.LeagueIds,
                param.MatchIds,
                await GetSelectedBetTypeGroups(param),
                AppSettings.WagerMonitoringPageSize,
                param.MinTransId);

            var tickets = await dynamicRepository.FetchAsync<TicketDto>(criteria);
            param.MinTransId = criteria.MinTransId;

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

        public async Task<IEnumerable<MatchInfo>> GetOtherMatches()
            => await ConfiguredCacheService.GetOrSetAsync(
                OtherMatchesCacheKey,
                () => dynamicRepository.Fetch<MatchInfo>(new GetOtherMatchCriteria(AppSettings.IsTest)),
                new CacheItemOptions().SetAbsoluteExpiration(
                    TimeSpan.FromMinutes(AppSettings.WagerMonitoringExpirationCacheInMinutes)));

        private async Task<IEnumerable<MatchInfo>> GetOutrightMatches()
            => await ConfiguredCacheService.GetOrSetAsync(
                OutrightMatchesCacheKey,
                () => dynamicRepository.Fetch<MatchInfo>(new GetOutrightMatchCriteria(AppSettings.IsTest)),
                new CacheItemOptions().SetAbsoluteExpiration(
                    TimeSpan.FromMinutes(AppSettings.WagerMonitoringExpirationCacheInMinutes)));

        private async Task<string> GetSelectedBetTypeGroups(WagerMonitoringParam param)
        {
            var displayBetTypeGroupIds = param.DisplayBetTypeGroupIds.ToListInt();
            var betTypeGroups = await GetBetTypeGroupsBySport(param.SportGroupId);

            return betTypeGroups
                .Where(x => displayBetTypeGroupIds.Contains(x.DisplayBetTypeGroup))
                .Select(x => x.BetTypeGroup)
                .JoinWithComma();
        }

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
                () => dynamicRepository.Fetch<MonitorBetTypeGroup>(new GetMonitorBetTypeGroupCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));

        private static bool IsOneXTwoMoneyline(MonitorBetTypeGroup betTypeGroup)
            => betTypeGroup.SportTypeGroup == 0 && betTypeGroup.DisplayBetTypeGroup == 5;

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