using CN88.Features.Site.Reports.Requests;

namespace CN88.Features.Site.Reports.ViewModels
{
    public class ReportViewModel
    {
        public ReportViewModel(
            ReportQuery query)
        {
            Query = query;
        }

        public ReportQuery Query { get; }

        public string Tile { get; }
    }
}