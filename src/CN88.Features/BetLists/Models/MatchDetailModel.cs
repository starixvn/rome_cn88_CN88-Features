namespace CN88.Features.BetLists.Models
{
    public class MatchDetailModel
    {
        public MatchDetailModel(long serial, int homeId, int awayId)
        {
            Serial = serial;
            HomeId = homeId;
            AwayId = awayId;
        }

        public long Serial { get; }

        public int HomeId { get; }

        public int AwayId { get; }
    }
}