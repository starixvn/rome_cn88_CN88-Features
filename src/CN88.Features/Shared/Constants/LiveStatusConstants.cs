namespace CN88.Features.Shared.Constants
{
    using System.Collections.Generic;

    public static class LiveStatusConstants
    {
        public const int PreMatch = 0;
        public const int InPlay = 1;
        public const int PreMatchAndInPlay = 2;

        public static IDictionary<int, string> LiveStatuses { get; } = new Dictionary<int, string>
        {
            [PreMatchAndInPlay] = nameof(PreMatchAndInPlay),
            [PreMatch] = nameof(PreMatch),
            [InPlay] = nameof(InPlay)
        };
    }
}