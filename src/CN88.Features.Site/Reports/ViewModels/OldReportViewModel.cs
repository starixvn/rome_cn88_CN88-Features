namespace CN88.Features.Site.Reports.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Features.Reports.Models;
    using Requests;
    using Shared.Helpers;
    using Shared.ViewModels;
    using Shared.ViewModels.Components;

    public class OldReportViewModel : ReportBaseViewModel
    {
        private const string OldReportMainActionPath = "~/OldReports/Index";

        public OldReportViewModel(
            OldReportQuery query,
            IEnumerable<SelectListItemExt> sportGroups,
            IEnumerable<ReportModel> reportData,
            ReportModel grandTotal,
            IEnumerable<AccountingPeriod> oldAccountingPeriods,
            bool isMobile)
            : base(query, reportData, grandTotal, isMobile)
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
                dateRangePickerViewModel,
                isMobile);
        }

        public OldReportFilterViewModel OldReportFilterViewModel { get; }

        protected override string GetReportMainActionPath() => OldReportMainActionPath;

        protected override bool IsOldReport() => true;
    }
}