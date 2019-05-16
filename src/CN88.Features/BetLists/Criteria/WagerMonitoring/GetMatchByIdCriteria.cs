namespace CN88.Features.BetLists.Criteria.WagerMonitoring
{
    using Fanex.Data.Repository;

    public class GetMatchByIdCriteria : CriteriaBase
    {
        public GetMatchByIdCriteria(int matchId)
        {
            MatchId = matchId;
        }

        public int MatchId { get; }

        public override string GetSettingKey() => "CS_Monitor_MatchSelByID";

        public override bool IsValid() => MatchId > 0;
    }
}