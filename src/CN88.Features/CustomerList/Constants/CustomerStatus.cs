using System.Collections.Generic;

namespace CN88.Features.CustomerList
{
    public static class CustomerStatus
    {
        public const int All = 0;
        public const int Active = 4;
        public const int ViewOnly = 3;
        public const int Suspended = 2;
        public const int Inactive = 1;

        public static IDictionary<int, string> Statuses { get; } = new Dictionary<int, string>
        {
            [All] = nameof(All),
            [Active] = nameof(Active),
            [ViewOnly] = nameof(ViewOnly),
            [Suspended] = nameof(Suspended),
            [Inactive] = nameof(Inactive)
        };
    }
}