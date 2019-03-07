namespace CN88.Features.Shared.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AppResources;

    public class SportGroup
    {
        private const int ParlaySportGroupId = 9;
        private readonly Func<int, string> getSportGroupDisplayName;

        public SportGroup()
        {
        }

        public SportGroup(
            int id,
            IEnumerable<int> displaySportIds)
        {
            Id = id;
            DisplaySportIds = displaySportIds;
        }

        internal SportGroup(
            int id,
            IEnumerable<int> displaySportIds,
            Func<int, string> getSportGroupDisplayName)
            : this(id, displaySportIds)
        {
            this.getSportGroupDisplayName = getSportGroupDisplayName;
        }

        public int Id { get; }

        public IEnumerable<int> DisplaySportIds { get; }

        public string DisplayName => GetDisplayName();

        private string GetDisplayName()
        {
            if (Id == ParlaySportGroupId)
            {
                return SharedResources.Parlay;
            }

            var stringBuilder = new StringBuilder();
            var length = DisplaySportIds.Count();

            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append($"{getSportGroupDisplayName(DisplaySportIds.ElementAt(i))}");

                if (i != length - 1)
                {
                    stringBuilder.Append(" / ");
                }
            }

            return stringBuilder.ToString();
        }
    }
}