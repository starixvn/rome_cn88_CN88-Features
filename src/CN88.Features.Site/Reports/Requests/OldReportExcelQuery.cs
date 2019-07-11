using CN88.Features.Models;

namespace CN88.Features.Site.Reports.Requests
{
    public class OldReportExcelQuery : ReportExcelBaseQuery
    {
        public OldReportExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}