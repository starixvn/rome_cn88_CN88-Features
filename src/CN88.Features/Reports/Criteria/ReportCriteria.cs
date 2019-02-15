namespace CN88.Features.Reports.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class ReportCriteria : CriteriaBase
    {
        public ReportCriteria(
            int reportType,
            DateTime fromDate,
            DateTime toDate,
            int sportTypeId,
            int betTypeId)
        {
            ReportType = reportType;
            FromDate = fromDate;
            ToDate = toDate;
            SportTypeId = sportTypeId;
            BetTypeId = betTypeId;
        }

        public int ReportType { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int SportTypeId { get; }

        public int BetTypeId { get; }

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