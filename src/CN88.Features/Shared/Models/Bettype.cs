using System;
using CN88.Features.Shared.AppResources;

namespace CN88.Features.Shared.Models
{
    public class BetType
    {
        public BetType()
        {
        }

        public BetType(int id, string name, bool isLive)
        {
            Id = id;
            Name = name;
            IsLive = isLive;
        }

        internal BetType(int id, string name, bool isLive, Func<int, string> getBetTypeDisplayName)
            : this(id, name, isLive)
        {
            DisplayName = (isLive ? $"({SharedResources.InPlay}) " : string.Empty) + getBetTypeDisplayName(id);
        }

        public int Id { get; }

        public string Name { get; }

        public bool IsLive { get; }

        public string DisplayName { get; }
    }
}