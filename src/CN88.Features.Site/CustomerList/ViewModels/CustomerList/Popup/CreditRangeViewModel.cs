namespace CN88.Features.Site.CustomerList.ViewModels
{
    using CN88.Features.CustomerList.Models;
    using CN88.Features.Shared.Models;

    public class CreditRangeViewModel
    {
        public CreditRangeViewModel(CreditPopupQuery query, CreditRangeModel creditModel)
        {
            CreditModel = creditModel;
            CustLevel = query.CustLevel;
            CustId = query.CustId;
            IsDisableSaveButton = query.IsInternal || !query.AccountManagementEditView;
        }

        public CreditRangeModel CreditModel { get; }

        public UserLevel CustLevel { get; }

        public int CustId { get; }

        public bool IsDisableSaveButton { get; }
    }
}