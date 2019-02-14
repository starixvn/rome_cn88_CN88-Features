using System;

namespace CN88.Features.Shared.Models
{
    public class Sport
    {
        public Sport()
        {
        }

        public Sport(int id, string name)
        {
            Id = id;
            Name = name;
        }

        internal Sport(int id, string name, Func<int, string> getSportDisplayName)
            : this(id, name)
        {
            DisplayName = getSportDisplayName == null ? name : getSportDisplayName(id);
        }

        public int Id { get; }

        public string Name { get; }

        public string DisplayName { get; }
    }
}