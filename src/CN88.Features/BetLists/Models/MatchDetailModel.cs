namespace CN88.Features.BetLists.Models
{
    public class MatchDetailModel
    {
        public MatchDetailModel()
        {
        }

        public MatchDetailModel(long serial, int homeId, int awayId, bool isNeutral)
        {
            Serial = serial;
            HomeId = homeId;
            AwayId = awayId;
            IsNeutral = isNeutral;
        }

        public long Serial { get; }

        public int HomeId { get; }

        public int AwayId { get; }

        public bool IsNeutral { get; }
    }
}