namespace Alpha.CN88.Site.App_Start
{
    using Fanex.Data.Repository;
    using Fanex.Logging;
    using Fanex.RemotingRef;
    using global::CN88.Features.CustomerList.DataAccesses;
    using global::CN88.Features.CustomerList.Services;
    using global::CN88.Features.Models;
    using global::CN88.Features.Reports.Services;
    using global::CN88.Features.Shared.DataAccesses;
    using global::CN88.Features.Shared.Services;
    using global::CN88.Features.Site.Models;
    using global::CN88.Features.Site.Shared.Controllers;
    using MediatR;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Web.Mvc;

    public static class DependencyConfig
    {
        public static void Init()
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            RegisterMediator(container);

            RegisterDataAccesses(container);

            RegisterServices(container);

            // This is an extension method from the integration package.
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            // This is an extension method from the integration package as well.
            container.RegisterMvcIntegratedFilterProvider();

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void RegisterMediator(Container container)
        {
            var assemblies = GetAssemblies();
            container.RegisterSingleton<IMediator, Mediator>();
            container.Register(typeof(IRequestHandler<,>), assemblies);
            container.Register(typeof(IAsyncRequestHandler<,>), assemblies);
            container.Register(typeof(ICancellableAsyncRequestHandler<>), assemblies);

            container.RegisterCollection(typeof(INotificationHandler<>), assemblies);
            container.RegisterCollection(typeof(IAsyncNotificationHandler<>), assemblies);
            container.RegisterCollection(typeof(ICancellableAsyncNotificationHandler<>), assemblies);
            container.RegisterCollection(typeof(IPipelineBehavior<,>), assemblies);

            container.RegisterSingleton(new SingleInstanceFactory(container.GetInstance));
            container.RegisterSingleton(new MultiInstanceFactory(container.GetAllInstances));
        }

        private static IEnumerable<Assembly> GetAssemblies()
        {
            yield return typeof(FeatureBaseController).Assembly;
        }

        private static void RegisterDataAccesses(Container container)
        {
            container.RegisterSingleton<IDynamicRepository, DynamicRepository>();

            container.RegisterSingleton<CustomerListDataAccess, CustomerListDataAccessImpl>();
            container.RegisterSingleton<SportGroupDataAccess, SportGroupDataAccessImpl>();
        }

        private static void RegisterServices(Container container)
        {
            container.RegisterSingleton<IUserSession, UserSessionExt>();
            container.RegisterSingleton<IUser, User>();
            container.RegisterSingleton(Logger.Log);
            container.RegisterSingleton<IRemotingRefService, RemotingRefService>();
            container.RegisterSingleton<CacheRefService, CacheRefServiceImpl>();
            container.RegisterSingleton<ReportDateService, ReportDateServiceImpl>();

            container.Register<SportGroupService>(
                () => new SportGroupServiceImpl(
                    container.GetInstance<SportGroupDataAccess>(),
                    container.GetInstance<CacheRefService>().GetSportName),
                Lifestyle.Singleton);
            container.Register<BettypeService>(
                () => new BettypeServiceImpl(
                    container.GetInstance<IDynamicRepository>(),
                    container.GetInstance<CacheRefService>().GetBetTypeName),
                Lifestyle.Singleton);

            container.RegisterSingleton<CustomerListService, CustomerListServiceImpl>();

            container.RegisterSingleton<ReportService, ReportServiceImpl>();
        }
    }
}