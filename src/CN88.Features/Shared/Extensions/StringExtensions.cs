namespace CN88.Features.Shared.Extensions
{
    public static class StringExtensions
    {
        public static string FormatNumber(this decimal numberValue)
            => numberValue.ToString("#,##0.00");
    }
}