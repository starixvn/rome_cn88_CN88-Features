namespace CN88.Features.Site.Reports.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Features.Reports.Models;
    using Requests;
    using Shared.ViewModels.Components;

    public class OldReportViewModel : ReportBaseViewModel
    {
        private const string OldReportMainActionPath = "~/OldReports/Index";

        public OldReportViewModel(
            OldReportQuery query,
            IEnumerable<SelectListItem> sportGroups,
            IEnumerable<ReportModel> reportData,
            ReportModel grandTotal,
            IEnumerable<AccountingPeriod> oldAccountingPeriods)
            : base(query, reportData, grandTotal)
        {
            var sportGroupSelectionViewModel = new SingleSelectionViewModel(sportGroups, query.UserSelectedSportGroupId);

            var accountingPeriodSelectionViewModel = new AccountingPeriodSelectionViewModel(oldAccountingPeriods, query.UserSelectedPeriodId);

            var dateRangePickerViewModel = new DateRangePickerViewModel(oldAccountingPeriods.Last().FromDate, oldAccountingPeriods.First().ToDate, query.FromDate, query.ToDate);

            OldReportFilterViewModel = new OldReportFilterViewModel(
                query.ReportType,
                query.IsOwner,
                query.CustId,
                query.CustName,
                query.CustLevel,
                sportGroupSelectionViewModel,
                accountingPeriodSelectionViewModel,
                dateRangePickerViewModel);
        }

        public OldReportFilterViewModel OldReportFilterViewModel { get; }

        protected override string GetReportMainActionPath() => OldReportMainActionPath;
    }
}