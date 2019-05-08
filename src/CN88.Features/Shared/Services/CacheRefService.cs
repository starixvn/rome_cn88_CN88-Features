namespace CN88.Features.Shared.Services
{
    using System.Threading;
    using Fanex.RemotingRef;

    public interface CacheRefService
    {
        string GetSportName(int sportTypeId);

        string GetBetTypeName(int betTypeId);

        string GetLeagueName(int leagueId);

        string GetTeamName(int teamId);
    }

    public class CacheRefServiceImpl : CacheRefService
    {
        private readonly IRemotingRefService remotingRefService;

        public CacheRefServiceImpl(IRemotingRefService remotingRefService)
        {
            this.remotingRefService = remotingRefService;
        }

        public string GetBetTypeName(int betTypeId)
            => remotingRefService.GetBetTypeName(betTypeId.ToString(), LanguageCode);

        public string GetSportName(int sportTypeId)
            => remotingRefService.GetSportName(sportTypeId.ToString(), LanguageCode);

        public string GetLeagueName(int leagueId)
            => remotingRefService.GetLeagueName(leagueId.ToString(), LanguageCode);

        public string GetTeamName(int teamId)
            => remotingRefService.GetTeamName(teamId.ToString(), LanguageCode);

        private static string LanguageCode
            => Thread.CurrentThread.CurrentCulture.Name;
    }
}