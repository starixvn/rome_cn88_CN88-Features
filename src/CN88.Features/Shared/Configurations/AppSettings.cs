namespace CN88.Features.Shared.Configurations
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using Fanex.Common;

    public static class AppSettings
    {
        [ThreadStatic]
        private static NameValueCollection appSettingValues;

        internal static NameValueCollection AppSettingValues
        {
            get { return appSettingValues ?? ConfigurationManager.AppSettings; }

            set { appSettingValues = value; }
        }

        public static string StoredProcedurePath => GetConfigValue("StoredProcedurePath");

        public static string RemoteRefURL => GetConfigValue("RemoteRefURL");

        public static string FeaturesAppPath => GetConfigValue("FeaturesAppPath");

        public static bool IsTest => GetConfigValue("LocalTest") == "1";

        public static int NumberOfPeriod => Converter.ToInt(GetConfigValue("OldReports:NumberOfPeriod"));

        public static string DefaultIpAddress => GetConfigValue("DefaultIpAddress");

        public static string LocalIpAddress => GetConfigValue("LocalIpAddress");

        public static string ResultAppPath => GetConfigValue("ResultSiteVirtualPath") ?? "site-results";

        public static string TotalBetsForecastAppPath => GetConfigValue("ExTotalBetsForecastAppPath") ?? "ex-totalbetsforecast";

        public static string GetConfigValue(string configName) => AppSettingValues[configName];

        public static void Verify() => Require(StoredProcedurePath, nameof(StoredProcedurePath));

        private static void Require(string key, string keyName)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ConfigurationErrorsException($"{keyName} is null or whitespace");
            }
        }
    }
}