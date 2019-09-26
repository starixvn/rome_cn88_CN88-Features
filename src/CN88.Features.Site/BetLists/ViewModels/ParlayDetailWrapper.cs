namespace CN88.Features.Site.BetLists.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Features.BetLists.Models;
    using Features.BetLists.Resources;
    using Features.Extensions;
    using Helpers;

    public class ParlayDetailWrapper
    {
        private readonly IDictionary<string, string> statusToResources = new Dictionary<string, string>
        {
            [BetStatus.Running] = BetListsResources.Running,
            [BetStatus.Reject] = BetListsResources.Reject,
            [BetStatus.Won] = BetListsResources.Won,
            [BetStatus.Lose] = BetListsResources.Lose,
            [BetStatus.Draw] = BetListsResources.Draw,
            [BetStatus.Waiting] = BetListsResources.Waiting,
            [BetStatus.Void] = BetListsResources.Void,
            [BetStatus.Refund] = BetListsResources.Refund
        };

        public ParlayDetailWrapper(
            ParlayDetailModel parlayDetail,
            IEnumerable<MatchDetailModel> matchDetails,
            ITicketHelper ticketHelper,
            int divisor)
        {
            ParlayDetail = parlayDetail;
            MatchDetails = matchDetails;
            Odds = GetOdds();
            string statusResource;
            Status = statusToResources.TryGetValue(ParlayDetail.Status.ToLowerInvariant(), out statusResource) ? statusResource : string.Empty;
            Events = GetEvents(ticketHelper);
            WinLostDate = parlayDetail.WinLostDate.ToShortDateFormat();
            WinLost = ConvertByBetType.Winloss(parlayDetail.WinLost / divisor);
            Comm = (parlayDetail.PlayerComm / divisor).ToString("#,##0.0000");
            Stake = BetListHelpers.Stake(parlayDetail.Stake / divisor);
        }

        public ParlayDetailModel ParlayDetail { get; }

        public IEnumerable<MatchDetailModel> MatchDetails { get; }

        public string WinLostDate { get; }

        public string WinLost { get; }

        public string Comm { get; }

        public string Status { get; }

        public string Stake { get; }

        public string Odds { get; }

        public List<string> Events { get; }

        private List<string> GetEvents(ITicketHelper ticketHelper)
        {
            var events = new List<string>();

            var currentSerial = ParlayDetail.Serial;
            var matches = MatchDetails.Where(m => m.Serial == currentSerial);

            foreach (var match in matches)
            {
                events.Add($"{ticketHelper.GetTeamNameById(match.HomeId)} - vs - {ticketHelper.GetTeamNameById(match.AwayId)}");
            }

            return events;
        }

        private string GetOdds()
            => ConvertByBetType.CommValue(ParlayDetail.Odds);
    }
}
