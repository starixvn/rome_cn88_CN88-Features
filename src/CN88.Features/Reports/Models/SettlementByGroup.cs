namespace CN88.Features.Reports.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class SettlementByGroup
    {
        public SettlementByGroup(int id, string name, IList<SettlementModel> settlements)
        {
            Id = id;
            Name = name;
            Settlements = settlements;
        }

        public int Id { get; }

        public string Name { get; }

        public IList<SettlementModel> Settlements { get; }

        public bool IsComplete => !Settlements.Any(s => !s.IsComplete);
    }
}