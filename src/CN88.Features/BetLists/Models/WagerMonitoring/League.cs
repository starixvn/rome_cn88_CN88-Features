namespace CN88.Features.BetLists.Models.WagerMonitoring
{
    public class League
    {
        public League()
        {
        }

        public League(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }

        public string Name { get; }
    }
}