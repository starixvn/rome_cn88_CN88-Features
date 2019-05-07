namespace CN88.Features.Site.Reports.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Features.BetLists.Constants;
    using Features.Reports.Constants;
    using Features.Reports.Models;
    using Features.Shared.AppResources;
    using Features.Shared.Constants;
    using Requests;
    using Shared.Helpers;
    using Shared.ViewModels;
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
            IEnumerable<SelectListItemExt> sportGroups,
            IEnumerable<SelectListItemExt> betTypes,
            IEnumerable<ReportModel> reportData,
            ReportModel grandTotal,
            AccountingPeriod lastPeriod,
            bool isMobile,
            IEnumerable<SelectListItemExt> allBetTypes = null)
            : base(query, reportData, grandTotal)
        {
            var sportGroupSelectionViewModel = new SingleSelectionViewModel(sportGroups, query.UserSelectedSportGroupId);
            var dateRangePickerViewModel = new DateRangePickerViewModel(lastPeriod.FromDate, DateTime.MaxValue, query.FromDate, query.ToDate);
            var betTypeSelectionViewModel = new SingleSelectionViewModel(betTypes, query.UserSelectedBetTypeId);
            var allBetTypesSelectionViewModel = new SingleSelectionViewModel(allBetTypes, 0);

            var liveStatuses = LiveStatusConstants.LiveStatuses
                .Select(s => new SelectListItemExt
                {
                    Text = SharedResources.ResourceManager.GetString(s.Value),
                    Value = s.Key.ToString(),
                    Selected = s.Key == query.UserSelectedLiveStatusId
                });
            var liveStatusSelectionViewModel = new SingleSelectionViewModel(liveStatuses, query.UserSelectedLiveStatusId);
            IsMobile = isMobile;
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
                liveStatusSelectionViewModel,
                allBetTypesSelectionViewModel,
                isMobile);
        }

        public ReportFilterViewModel ReportFilterViewModel { get; }

        protected override string GetReportMainActionPath() => ReportMainActionPath;

        protected override bool IsOldReport() => false;

        public bool IsMobile { get; }

        public bool IsShowTotalRow()
            => ReportData.Any() 
                && (!IsMobile || (IsMobile && IsNotOwner));
    }
}