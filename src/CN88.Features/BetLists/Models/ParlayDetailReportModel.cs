namespace CN88.Features.BetLists.Models
{
    using System.Collections.Generic;

    public class ParlayDetailReportModel
    {
        public ParlayDetailReportModel(
            IEnumerable<ParlayDetailModel> parlayDetails,
            IEnumerable<MatchDetailModel> matchDetails,
            decimal outstanding)
        {
            ParlayDetails = parlayDetails;
            MatchDetails = matchDetails;
            Outstanding = outstanding;
        }

        public IEnumerable<ParlayDetailModel> ParlayDetails { get; }

        public IEnumerable<MatchDetailModel> MatchDetails { get; }

        public decimal Outstanding { get; }
    }
}
