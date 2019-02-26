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
        protected internal ReportParam CreateReportParam(ReportQuery message)
            => new ReportParam(
                message.CustId,
                message.IsOwner,
                message.FromDate,
                message.ToDate,
                message.UserSelectedSportGroupId,
                message.UserSelectedBetTypeId,
                message.ReportType,
                message.UserSelectedLiveStatusId,
                message.CustLevel);

        protected internal ReportModel AccumulateGrandTotal(IEnumerable<ReportModel> reportData)
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
    }
}