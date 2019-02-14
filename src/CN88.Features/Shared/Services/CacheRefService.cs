using System.Threading;
using Fanex.RemotingRef;

namespace CN88.Features.Shared.Services
{
    public interface CacheRefService
    {
        string GetSportName(int sportTypeId);

        string GetBetTypeName(int betTypeId);
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

        private static string LanguageCode
            => Thread.CurrentThread.CurrentCulture.Name;
    }
}