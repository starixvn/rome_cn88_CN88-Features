namespace CN88.Features.Site.Reports.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Features.BetLists.Constants;
    using Features.Reports.Constants;
    using Features.Reports.Models;
    using Features.Shared.AppResources;
    using Features.Shared.Constants;
    using Requests;
    using Shared.Helpers;
    using Shared.ViewModels.Components;

    public class ReportViewModel : ReportBaseViewModel
    {
        private const string ReportMainActionPath = "~/Reports/Index";

        private readonly IDictionary<int, int> BetListTypes = new Dictionary<int, int>
        {
            [ReportTypeConstants.SettledWager] = BetListTypeConstant.Settled,
            [ReportTypeConstants.UnsettledWager] = BetListTypeConstant.Unsettled,
            [ReportTypeConstants.CancelledReport] = BetListTypeConstant.Cancelled,
            [ReportTypeConstants.AbnormalBets] = BetListTypeConstant.Abnormal,
        };

        public ReportViewModel(
            ReportQuery query,
            IEnumerable<SelectListItem> sportGroups,
            IEnumerable<SelectListItem> betTypes,
            IEnumerable<ReportModel> reportData,
            ReportModel grandTotal,
            AccountingPeriod lastPeriod)
            : base(query, reportData, grandTotal)
        {
            var sportGroupSelectionViewModel = new SingleSelectionViewModel(sportGroups, query.UserSelectedSportGroupId);
            var dateRangePickerViewModel = new DateRangePickerViewModel(lastPeriod.FromDate, DateTime.MaxValue, query.FromDate, query.ToDate);
            var betTypeSelectionViewModel = new SingleSelectionViewModel(betTypes, query.UserSelectedBetTypeId);

            var liveStatuses = LiveStatusConstants.LiveStatuses
                .Select(s => new SelectListItem
                {
                    Text = SharedResources.ResourceManager.GetString(s.Value),
                    Value = s.Key.ToString(),
                    Selected = s.Key == query.UserSelectedLiveStatusId
                });
            var liveStatusSelectionViewModel = new SingleSelectionViewModel(liveStatuses, query.UserSelectedLiveStatusId);

            ReportFilterViewModel = new ReportFilterViewModel(
                query.DisplayMode,
                query.ReportType,
                query.IsOwner,
                query.CustId,
                query.CustName,
                JsonHelper.ToJsonFunc(query.BreadcrumbLevel),
                query.CustLevel,
                sportGroupSelectionViewModel,
                dateRangePickerViewModel,
                betTypeSelectionViewModel,
                liveStatusSelectionViewModel);
        }

        public ReportFilterViewModel ReportFilterViewModel { get; }

        protected override int GetBetListType(int reportType)
            => BetListTypes[reportType];

        protected override string GetReportMainActionPath() => ReportMainActionPath;
    }
}