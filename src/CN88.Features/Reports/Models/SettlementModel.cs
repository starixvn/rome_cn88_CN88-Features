namespace CN88.Features.Reports.Models
{
    public class SettlementModel
    {
        public SettlementModel(int id, string name, long unsettled, long settled)
        {
            Id = id;
            Name = name;
            Unsettled = unsettled;
            Settled = settled;
        }

        public int Id { get; }

        public string Name { get; }

        public long Unsettled { get; }

        public long Settled { get; }

        public bool IsComplete => Unsettled == 0;
    }
}
