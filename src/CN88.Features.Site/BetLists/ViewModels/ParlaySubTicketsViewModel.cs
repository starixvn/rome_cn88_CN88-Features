namespace CN88.Features.Site.BetLists.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using BetList.Core.Builders;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Features.BetLists.Models;
    using Requests;

    public class ParlaySubTicketsViewModel
    {
        public ParlaySubTicketsViewModel(
            GetParlaySubTicketsQuery query,
            BetListData betListData,
            IBetListCN88HtmlBuilder cn88Builder,
            IBetListHTMLBuilder builder,
            ITicketHelper ticketHelper)
        {
            BetListData = betListData;
            CN88Builder = cn88Builder;
            Builder = builder;
            TicketHelper = ticketHelper;

            ticketHelper.IsShowOddsWithChoice = true;
            HideOfferNameUnitStakes = string.IsNullOrEmpty(query.BetCheck);
            ParlayOfferNames = string.IsNullOrEmpty(query.BetCheck) 
                ? new List<string>() 
                : ticketHelper.GetCombinationMPOfferNameList(query.BetTeam, query.BetCheck, query.Handicap1, query.Handicap2);
            ParlaySingleTickets = betListData.Tickets
                .Where(t => t.Stake > 0)
                .OrderBy(t => t.MatchId)
                .ToList();

            UnitStakeByOfferNames = GetUnitStakeByOfferName(query);
        }

        public IDictionary<string, string> UnitStakeByOfferNames { get; }

        private IList<string> ParlayOfferNames { get; }

        private List<TicketDto> ParlaySingleTickets { get; }

        private bool HideOfferNameUnitStakes { get; }

        public BetListData BetListData { get; }

        public IBetListCN88HtmlBuilder CN88Builder { get; }

        public IBetListHTMLBuilder Builder { get; }

        public ITicketHelper TicketHelper { get; }

        public bool HasValidData
            => ParlaySingleTickets.Any() 
                && (HideOfferNameUnitStakes 
                    || ParlayOfferNames.Count <= ParlaySingleTickets.Count);

        private IDictionary<string, string> GetUnitStakeByOfferName(GetParlaySubTicketsQuery query)
        {
            var offerNameUnitStakes = new Dictionary<string, string>();
            if (HideOfferNameUnitStakes)
            {
                return offerNameUnitStakes;
            }

            var isSystemParlay = query.Handicap1 == 0;

            if (HasValidData)
            {
                var offerNamesAmount = ParlayOfferNames.Count;
                for (var i = 0; i < offerNamesAmount; i++)
                {
                    var comboTicket = isSystemParlay ? ParlaySingleTickets[0] : ParlaySingleTickets[i];

                    offerNameUnitStakes.Add(ParlayOfferNames[i], comboTicket.Currency + " " + comboTicket.Stake.ToString("N0"));
                }
            }

            return offerNameUnitStakes;
        }
    }
}