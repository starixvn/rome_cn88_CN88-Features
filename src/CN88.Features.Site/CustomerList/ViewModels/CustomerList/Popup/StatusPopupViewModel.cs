namespace CN88.Features.Site.CustomerList.ViewModels
{
    using CN88.Features.Shared.Models;

    public class StatusPopupViewModel
    {
        public StatusPopupViewModel(CustomerStatusPopupQuery query)
        {
            CustId = query.CustId;
            StatusId = query.StatusId;
            CustLevel = query.CustLevel;
            IsDisableSaveButton = query.IsInternal || !query.AccountManagementEditView;
        }

        public int CustId { get; }

        public int StatusId { get; }

        public UserLevel CustLevel { get; }

        public bool IsDisableSaveButton { get; }
    }
}