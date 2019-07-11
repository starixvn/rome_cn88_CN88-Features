namespace CN88.Features.Site.Reports.Requests
{
    using CN88.Features.Models;
    using CN88.Features.Site.Shared.ViewModels;
    using MediatR;

    public class ReportExcelBaseQuery : ReportBaseQuery, IRequest<ExcelFileViewModel>
    {
        public ReportExcelBaseQuery(IUser viewer)
            : base(viewer)
        {
        }
    }
}