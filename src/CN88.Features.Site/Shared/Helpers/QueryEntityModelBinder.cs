namespace CN88.Features.Site.Shared.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Features.Models;
    using Features.Reports.Models;
    using Newtonsoft.Json;
    using Requests;

    public class QueryEntityModelBinder : DefaultModelBinder
    {
        private static readonly Type[] IUserType = { typeof(IUser) };
        private static readonly Type BaseQueryType = typeof(BaseQuery);

        protected override object CreateModel(
            ControllerContext controllerContext,
            ModelBindingContext bindingContext,
            Type modelType)
        {
            if (modelType != null
                && BaseQueryType.IsAssignableFrom(modelType)
                && modelType.GetConstructor(IUserType) != null)
            {
                return CreateInstanceOfAssignableBaseQuery(modelType);
            }

            return CallBaseCreateModel(controllerContext, bindingContext, modelType);
        }

        protected virtual object CreateInstanceOfAssignableBaseQuery(Type modelType)
            => Activator.CreateInstance(modelType, DependencyResolver.Current.GetService<IUser>());

        protected virtual object CallBaseCreateModel(
           ControllerContext controllerContext,
           ModelBindingContext bindingContext,
           Type modelType) => base.CreateModel(controllerContext, bindingContext, modelType);

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var boundModel = CallBaseBindModel(controllerContext, bindingContext);

            var reportBaseQuery = boundModel as BaseQuery;
            if (reportBaseQuery != null)
            {
                var breadcrumbLevelJson = GetBreadcrumbLevelJson(controllerContext);

                if (!string.IsNullOrWhiteSpace(breadcrumbLevelJson)
                   && breadcrumbLevelJson.LastIndexOf(",[", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    reportBaseQuery.BreadcrumbLevel = JsonConvert.DeserializeObject<IEnumerable<CustInfo>>(
                        breadcrumbLevelJson,
                        new JsonSerializerSettings
                        {
                            Error = (sender, errorArgs) => { errorArgs.ErrorContext.Handled = true; }
                        });
                }

                return reportBaseQuery;
            }

            return boundModel;
        }

        protected virtual object CallBaseBindModel(
           ControllerContext controllerContext,
           ModelBindingContext bindingContext)
            => base.BindModel(controllerContext, bindingContext);

        protected virtual string GetBreadcrumbLevelJson(ControllerContext controllerContext)
            => controllerContext.RequestContext.HttpContext.Request["BreadcrumbLevelJson"];
    }
}