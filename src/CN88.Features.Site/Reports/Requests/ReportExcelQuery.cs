namespace CN88.Features.Site.Reports.Requests
{
    using Features.Models;

    public class ReportExcelQuery : ReportExcelBaseQuery
    {
        public ReportExcelQuery(IUser viewer)
            : base(viewer)
        {
        }
    }
}