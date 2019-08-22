using System.Collections.Generic;

namespace CN88.Features.Shared.Constants
{
    public static class SportGroupTypeConstants
    {
        public const int AllSports = -1;
        public const int NonSports = 0;
        public const int Sportsbook = 1;        

        public static IDictionary<int, string> SportGroups { get; } = new Dictionary<int, string>
        {
            [AllSports] = nameof(AllSports),
            [Sportsbook] = nameof(Sportsbook),
            [NonSports] = nameof(NonSports),
        };
    }
}