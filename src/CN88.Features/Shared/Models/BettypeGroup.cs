namespace CN88.Features.Shared.Models
{
    using System;

    public class BetType
    {
        public BetType()
        {
        }

        public BetType(int id, string name)
        {
            Id = id;
            Name = name;
        }

        internal BetType(int id, string name, Func<int, string> getBetTypeDisplayName)
            : this(id, name)
        {
            DisplayName = getBetTypeDisplayName == null ? name : getBetTypeDisplayName(id);
        }

        public int Id { get; }

        public string Name { get; }

        public string DisplayName { get; }
    }
}