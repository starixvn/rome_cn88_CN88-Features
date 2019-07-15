namespace CN88.Features.Site.Reports.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using Features.Reports.Models;
    using Features.Reports.Params;
    using Requests;
    using Shared;

    public class ReportsBaseHandler : BaseHandler
    {
        protected internal static ReportParam CreateReportParam(ReportBaseQuery message)
            => new ReportParam(
                message.CustId,
                message.SubAccountId,
                message.IsOwner,
                message.ActorLevel.IsHeadline() || message.ViewerLevelId > message.CustLevelId,
                message.FromDate,
                message.ToDate,
                message.UserSelectedSportGroupId,
                message.UserSelectedBetTypeId,
                message.ReportType,
                message.UserSelectedLiveStatusId,
                message.CustLevel);

        protected internal static ReportModel AccumulateGrandTotal(IEnumerable<ReportModel> reportData)
        {
            var grandTotal = new ReportModel();

            if (reportData != null && reportData.Any())
            {
                foreach (var data in reportData)
                {
                    grandTotal.Add(data);
                }
            }

            return grandTotal;
        }

        protected internal static IEnumerable<AccountingPeriodGroup> BuildAccountingPeriodGroup(IEnumerable<AccountingPeriod> periods)
        {
            var groups = periods
                .OrderBy(p => p.PeriodId)
                .GroupBy(p => p.PeriodId.ToString());
            var result = new List<AccountingPeriodGroup>();

            foreach (var group in groups)
            {
                var ps = new List<AccountingPeriod>();

                foreach (var p in group)
                {
                    ps.Add(p);
                }

                result.Add(new AccountingPeriodGroup(group.Key, ps.OrderByDescending(p => p.Year).ToList()));
            }

            return result;
        }
    }
}