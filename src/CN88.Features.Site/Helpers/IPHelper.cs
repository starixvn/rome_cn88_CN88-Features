namespace CN88.Features.Site.Helpers
{
    using System.Web;

    public static class IPHelper
    {
        public static string CustomerIp()
        {
            var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            return ip.Split(',')[0];
        }
    }
}