namespace CN88.Features.Site.Reports.Requests
{
    using Features.Models;
    using MediatR;
    using Shared.ViewModels;

    public class ReportExcelQuery : ReportQuery, IRequest<ExcelFileViewModel>
    {
        public ReportExcelQuery(IUser viewer)
            : base(viewer)
        {
        }
    }
}