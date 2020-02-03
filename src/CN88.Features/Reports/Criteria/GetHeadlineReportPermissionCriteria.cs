using Fanex.Data.Repository;

namespace CN88.Features.Reports.Criteria
{
    public class GetHeadlineReportPermissionCriteria : CriteriaBase
    {
        public GetHeadlineReportPermissionCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public override bool IsValid() => CustId > 0;

        public override string GetSettingKey() => "CS_HL_ReportPermission_Sel";
    }
}