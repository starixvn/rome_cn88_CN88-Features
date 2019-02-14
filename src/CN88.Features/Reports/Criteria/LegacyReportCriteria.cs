namespace CN88.Features.Reports.Criteria
{
    using System;

    public class LegacyReportCriteria : ReportBaseCriteria
    {
        public LegacyReportCriteria(
            int reportType,
            DateTime fromDate,
            DateTime toDate,
            int sportTypeId,
            int betTypeId)
            : base(reportType, fromDate, toDate, sportTypeId, betTypeId)
        {
        }

        public override string GetSettingKey()
        {
            throw new NotImplementedException();
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}