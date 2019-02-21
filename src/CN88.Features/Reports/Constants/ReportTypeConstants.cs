using System.Collections.Generic;

namespace CN88.Features.Reports.Constants
{
    public static class ReportTypeConstants
    {
        public const int SettledWager = 1;
        public const int UnsettledWager = 2;
        public const int CancelledReport = 3;
        public const int AbnormalBets = 4;

        public static IDictionary<int, string> WagerReportTypes { get; } = new Dictionary<int, string>
        {
            [SettledWager] = nameof(SettledWager),
            [UnsettledWager] = nameof(UnsettledWager),
        };

        public static IDictionary<int, string> CancelledReportTypes { get; } = new Dictionary<int, string>
        {
            [CancelledReport] = nameof(CancelledReport),
            [AbnormalBets] = nameof(AbnormalBets),
        };
    }
}