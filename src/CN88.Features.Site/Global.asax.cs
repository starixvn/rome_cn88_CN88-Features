namespace CN88.Features.Site
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Alpha.Auth.Filters;
    using Alpha.CN88.Site.App_Start;
    using Alpha.Web;
    using Alpha.Web.Filters;
    using Alpha.Web.Helpers;
    using global::CN88.CustomerSetting.Client;
    using Fanex.Data;
    using Fanex.Data.CrossCutting.WrappingException;
    using Fanex.Data.OldConnectionStringProvider;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.Logging.RabbitMQ;
    using Fanex.Logging.Sentry.RavenClient;
    using Fanex.RemotingRef;
    using Features.Shared.Configurations;
    using ONEbook.UM.Filters;
    using Shared.Helpers;
    using SharpRaven;
    using Sunplus.Agent.Model.Entities;
    using ExceptionHandler = Fanex.Logging.Extensions.Web.ExceptionHandler;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            MvcHandler.DisableMvcResponseHeader = true;

            AreaRegistration.RegisterAllAreas();
            CN88CustomerSettingRegistration.Init();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            InitLogging();
            DependencyConfig.Init();
            AppSettings.Verify();

            DbSettingProviderManager
                .StartNewSession()
                .Use(new OldConnectionStringProvider())
                .UseDefaultDbSettingProvider(resourcePath: AppSettings.StoredProcedurePath)
                .Use((settingKey, commandTimeout) => new WrappingExceptionObjectDb(settingKey, commandTimeout))
                .Run();

            ShareSession.Init();

            ModelBinders.Binders.DefaultBinder = new QueryEntityModelBinder();
            GlobalFilters.Filters.Add(new CheckUMFilter());
            GlobalFilters.Filters.Add(new CheckBelongingFilter());
            GlobalFilters.Filters.Add(new NoCacheFilter());

            StaticResources.Init(GetBuildVersion());

            RefDataTypeContainer
                .ConnectTo(new Uri(AppSettings.RemoteRefURL))
                .FromApplication("CN88.Features.Site");
        }

        /// <summary>
        /// Application error event handler.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            //if (Request.IsLocal)
            //{
            //    return;
            //}

            Exception ex = Server.GetLastError();
            var currentCustomer = UserSession.Current;

            if (ex is FormatException &&
                Request.Params.AllKeys.Any(key => Alpha.Auth.Configurations.AuthorizationSection.Instance.CustIdQueryStringParamNames.Any(cf => string.Equals(key, cf, StringComparison.OrdinalIgnoreCase))))
            {
                return;
            }

            ExceptionHandler.Handle(ex,
                new Dictionary<string, object>
                {
                    ["CustId"] = currentCustomer?.CustId,
                    ["CustName"] = currentCustomer?.UserName,
                    ["SubAccountName"] = currentCustomer?.SubUserName,
                    ["AdminName"] = currentCustomer?.AdminName
                });
        }

        private void InitLogging()
        {
            var logEngines = new List<ILogEngine>
            {
                new RabbitMQLogging(ConfigurationManager.AppSettings["Fanex.Logging:RabbitMQConnectionStrings"])
            };

            var sentryUrl = ConfigurationManager.AppSettings["Fanex.Logging:SentryUrl"];
            if (!string.IsNullOrWhiteSpace(sentryUrl))
            {
                logEngines.Add(new SentryLogging(new SentryEngineOptions
                {
                    Dsn = new Dsn(sentryUrl)
                }));
            }

            LogManager
                    .SetDefaultLogCategory(ConfigurationManager.AppSettings["Fanex.Logging:DefaultCategory"])
                    .Use(logEngines);

            ExceptionHandler
                .UseConfig(new LoggingConfiguration()
                .GetHttpHeaderInfo()
                .UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo));
        }

        private static string GetBuildVersion()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            return versionInfo.FileVersion;
        }
    }
}