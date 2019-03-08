namespace CN88.Features.Shared.Models
{
    using System;

    public class BetType
    {
        public BetType()
        {
        }

        public BetType(int sportTypeGroup, int id, string name)
        {
            SportTypeGroup = sportTypeGroup;
            Id = id;
            Name = name;
        }

        internal BetType(int sportTypeGroup, int id, string name, Func<int, string> getBetTypeDisplayName)
            : this(sportTypeGroup, id, name)
        {
            DisplayName = getBetTypeDisplayName == null ? name : getBetTypeDisplayName(id);
        }

        public int SportTypeGroup { get; }

        public int Id { get; }

        public string Name { get; }

        public string DisplayName { get; }
    }
}