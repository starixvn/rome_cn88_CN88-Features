namespace CN88.Features.Site.BetLists.Helpers
{
    public static class BetListHelpers
    {
        public static string Stake(object value)
            => string.Format("{0:#,##0.00}", value);
    }
}