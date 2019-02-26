namespace CN88.Features.Shared.Utils
{
    using System;

    public static class SystemTime
    {
        [ThreadStatic]
        private static DateTime? today;

        public static DateTime Today
        {
            get { return today ?? DateTime.Today; }
            internal set { today = value; }
        }

        public static DateTime Yesterday => Today.AddDays(-1);
    }
}