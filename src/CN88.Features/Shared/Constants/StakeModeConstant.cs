namespace CN88.Features.Shared.Constants
{
    using System.Collections.Generic;

    public static class StakeModeConstant
    {
        public const int GreaterThan = 1;
        public const int LessThan = 2;
        public const int Equal = 0;
        public const int LessThanOrEqual = 3;
        public const int GreaterThanOrEqual = 4;

        public static IEnumerable<int> Modes { get; } = new List<int> { GreaterThan, LessThan, Equal };
    }
}
