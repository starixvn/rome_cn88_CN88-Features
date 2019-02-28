namespace CN88.Features.Reports.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetPTHistoryCriteria : CriteriaBase
    {
        public GetPTHistoryCriteria(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            bool isTest)
        {
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
            IsTest = isTest;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public bool IsTest { get; }

        public override string GetSettingKey() => "CS_CustSetting_LegacyReportSettingHistory_Sel";

        public override bool IsValid() => CustId > 0 && FromDate <= ToDate;
    }
}