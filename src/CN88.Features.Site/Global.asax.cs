namespace CN88.Features.Site
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Alpha.Auth.Filters;
    using Alpha.Web;
    using Alpha.Web.Filters;
    using Alpha.Web.Helpers;
    using Fanex.Data;
    using Fanex.Data.CrossCutting.WrappingException;
    using Fanex.Data.OldConnectionStringProvider;
    using Features.Shared.Configurations;
    using ONEbook.UM.Filters;
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
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            AppSettings.Verify();

            DbSettingProviderManager
                .StartNewSession()
                .Use(new OldConnectionStringProvider())
                .UseDefaultDbSettingProvider(resourcePath: AppSettings.StoredProcedurePath)
                .Use((settingKey, commandTimeout) => new WrappingExceptionObjectDb(settingKey, commandTimeout))
                .Run();

            ShareSession.Init();

            GlobalFilters.Filters.Add(new CheckUMFilter());
            GlobalFilters.Filters.Add(new CheckBelongingFilter());
            GlobalFilters.Filters.Add(new NoCacheFilter());

            StaticResources.Init(GetBuildVersion());
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

        private static string GetBuildVersion()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            return versionInfo.FileVersion;
        }
    }
}