namespace CN88.Features.Shared.Constants
{
    using System.Collections.Generic;

    public static class StakeModeConstant
    {
        public const int LessThan = 1;
        public const int GreaterThan = 2;
        public const int Equal = 3;
        public const int LessThanOrEqual = 4;
        public const int GreaterThanOrEqual = 5;

        public static IEnumerable<int> Modes =
            new List<int> { GreaterThanOrEqual, GreaterThan, Equal, LessThanOrEqual, LessThan };
    }
}
