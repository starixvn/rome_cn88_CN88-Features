namespace CN88.Features.Site.CustomerList.ViewModels
{
    using CN88.Features.CustomerList.Models;
    using CN88.Features.Shared.Models;

    public class CreditRangeViewModel
    {
        public CreditRangeViewModel(CreditPopupQuery query, CreditRangeModel creditModel)
        {
            CreditModel = creditModel;
            CustId = query.CustId;
        }

        public CreditRangeModel CreditModel { get; }

        public int CustId { get; }
    }
}