namespace Alpha.CN88.Site.App_Start
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web.Mvc;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.RemotingRef;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Data.Repository;
    using Fanex.RemotingRef;
    using global::CN88.BetList.Core.Builders;
    using global::CN88.Features.BetLists;
    using global::CN88.Features.BetLists.DataAccesses;
    using global::CN88.Features.CustomerList;
    using global::CN88.Features.CustomerList.DataAccesses;
    using global::CN88.Features.CustomerList.Services;
    using global::CN88.Features.Models;
    using global::CN88.Features.Reports.Services;
    using global::CN88.Features.Shared.DataAccesses;
    using global::CN88.Features.Shared.Services;
    using global::CN88.Features.Site.BetLists.Workbooks;
    using global::CN88.Features.Site.Models;
    using global::CN88.Features.Site.Reports.Handlers;
    using global::CN88.Features.Site.Shared.Controllers;
    using global::CN88.Features.Site.Shared.Helpers;
    using global::CN88.Features.SubAccount.Services;
    using MediatR;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using SmartPunter.DataAccesses;
    using SmartPunter.Services;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;
    using Web.Helpers;
    using Web.Helpers.Impl;

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
            container.Register<ISmartPunterDataAccess, SmartPunterDataAccess>(Lifestyle.Singleton);
        }

        private static void RegisterServices(Container container)
        {
            container.RegisterSingleton<IUserSession, UserSessionExt>();
            container.RegisterSingleton<IUser, User>();
            container.RegisterSingleton(Fanex.Logging.Logger.Log);
            container.RegisterSingleton<IRemotingRefService, RemotingRefService>();
            container.RegisterSingleton<CacheRefService, CacheRefServiceImpl>();
            container.RegisterSingleton<Encryptor, EncryptorImpl>();
            container.RegisterSingleton<ReportDateService, ReportDateServiceImpl>();

            container.RegisterPerWebRequest<SessionManager, SessionManagerImpl>();

            container.RegisterSingleton<SportGroupService, SportGroupServiceImpl>();
            container.RegisterSingleton<BettypeGroupService, BettypeGroupServiceImpl>();

            container.RegisterSingleton<CustomerListService, CustomerListServiceImpl>();
            container.RegisterSingleton<MemberStatementService, MemberStatementServiceImpl>();

            container.RegisterSingleton<AccountingPeriodService, AccountingPeriodServiceImpl>();
            container.RegisterSingleton<SettlementSummaryService, SettlementSummaryServiceImpl>();
            container.RegisterSingleton<ReportService, ReportServiceImpl>();
            container.RegisterSingleton<PasswordService, PasswordServiceImpl>();

            container.RegisterPerWebRequest<SubAccountService, SubAccountServiceImpl>();
            container.RegisterPerWebRequest<SubAccountValidator, SubAccountValidatorImpl>();

            container.RegisterSingleton<ISmartPunterService, SmartPunterService>();
            container.Register(
                () => SmartPunterBetListMonitor.CreateInstance(
                    container.GetInstance<IUserSession>(),
                    container.GetInstance<ISmartPunterService>(),
                    IPHelper.CustomerIP),
                Lifestyle.Singleton);
            container.RegisterSingleton<BetListsService, BetListsServiceImpl>();

            container.Register(typeof(Workbook<>), typeof(NpoiWorkbook<>), Lifestyle.Scoped);

            container.RegisterSingleton<ReportsWorkbook, ReportsWorkbookImpl>();

            container.RegisterSingleton<ICacheRef, CacheRef>();

            RegisterBetListCore(container);
        }

        private static void RegisterBetListCore(Container container)
        {
            container.RegisterPerWebRequest<ITicketHelper, TicketHelper>();
            container.RegisterPerWebRequest<IBetListCN88HtmlBuilder, BetListCN88HtmlBuilder>();
            container.RegisterPerWebRequest<IBetListHTMLBuilder, BetListHTMLBuilder>();

            container.RegisterPerWebRequest<IBetListFactory<BetListExcelBuilder>, BetListExcelFactory>();

            container.RegisterPerWebRequest<IBetListExcelBuilder>(() => container.GetInstance<IBetListFactory<BetListExcelBuilder>>().CreateBuilder());
            container.RegisterPerWebRequest<IBetListCN88ExcelBuilder, BetListCN88ExcelBuilder>();

            container.RegisterSingleton<ParlayDetailWorkbook, ParlayDetailWorkbookImpl>();
            container.RegisterSingleton<BetListOfReportWorkbook, BetListOfReportWorkbookImpl>();
        }
    }
}