namespace CN88.Features.Site.Reports.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Features.BetLists.Constants;
    using Features.Reports.Constants;
    using Features.Reports.Models;
    using Requests;
    using Shared.Helpers;
    using Shared.ViewModels.Components;
    using Shared.ViewModels;

    public class OldReportViewModel : ReportBaseViewModel
    {
        private const string OldReportMainActionPath = "~/OldReports/Index";

        private readonly IDictionary<int, int> OldBetListTypes = new Dictionary<int, int>
        {
            [ReportTypeConstants.SettledWager] = BetListTypeConstant.OldSettled,
            [ReportTypeConstants.UnsettledWager] = BetListTypeConstant.Unsettled,
            [ReportTypeConstants.CancelledReport] = BetListTypeConstant.OldCancelled,
            [ReportTypeConstants.AbnormalBets] = BetListTypeConstant.OldAbnormal,
        };

        public OldReportViewModel(
            OldReportQuery query,
            IEnumerable<SelectListItemExt> sportGroups,
            IEnumerable<ReportModel> reportData,
            ReportModel grandTotal,
            IEnumerable<AccountingPeriod> oldAccountingPeriods)
            : base(query, reportData, grandTotal)
        {
            var sportGroupSelectionViewModel = new SingleSelectionViewModel(sportGroups, query.UserSelectedSportGroupId);

            var accountingPeriodSelectionViewModel = new AccountingPeriodSelectionViewModel(oldAccountingPeriods, query.UserSelectedPeriodId);

            var dateRangePickerViewModel = new DateRangePickerViewModel(oldAccountingPeriods.Last().FromDate, oldAccountingPeriods.First().ToDate, query.FromDate, query.ToDate);

            OldReportFilterViewModel = new OldReportFilterViewModel(
                query.DisplayMode,
                query.ReportType,
                query.IsOwner,
                query.CustId,
                query.CustName,
                JsonHelper.ToJsonFunc(query.BreadcrumbLevel),
                query.CustLevel,
                sportGroupSelectionViewModel,
                accountingPeriodSelectionViewModel,
                dateRangePickerViewModel);
        }

        public OldReportFilterViewModel OldReportFilterViewModel { get; }

        protected override int GetBetListType(int reportType)
            => OldBetListTypes[reportType];

        protected override string GetReportMainActionPath() => OldReportMainActionPath;
    }
}