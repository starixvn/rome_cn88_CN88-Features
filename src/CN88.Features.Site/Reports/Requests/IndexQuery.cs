namespace CN88.Features.Site.Reports.Requests
{
    using CN88.Features.Reports.Models;
    using Features.Models;
    using MediatR;
    using ViewModels;

    public class IndexQuery : ReportBaseQuery, IRequest<IndexViewModel>
    {
        public IndexQuery(IUser viewer)
            : base(viewer)
        {
        }
    }

    public class SettlementSummaryQuery : ReportBaseQuery, IRequest<SettlementSummaryReport>
    {
        public SettlementSummaryQuery(IUser viewer)
            : base(viewer)
        {
        }
    }

    public class AccountingSheetQuery : ReportBaseQuery, IRequest<AccountingSheetViewModel>
    {
        public AccountingSheetQuery(IUser viewer)
            : base(viewer)
        {
        }
    }
}