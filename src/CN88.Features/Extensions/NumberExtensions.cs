namespace CN88.Features.Extensions
{
    public static class NumberExtensions
    {        
        public static string ToNumberWithoutDecimal(this int value)
            => value.ToString("#,##0");

        public static string ToNumberWithoutDecimal(this decimal value)
            => value.ToString("#,##0");

        public static decimal Round(this decimal value)
            => decimal.Round(value, 2);
    }
}
