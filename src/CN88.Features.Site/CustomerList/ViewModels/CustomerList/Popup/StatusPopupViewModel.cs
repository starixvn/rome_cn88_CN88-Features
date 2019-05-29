namespace CN88.Features.Site.CustomerList.ViewModels
{
    public class StatusPopupViewModel
    {
        public StatusPopupViewModel(CustomerStatusPopupQuery query)
        {
            CustId = query.CustId;
            StatusId = query.StatusId;
        }

        public int CustId { get; }

        public int StatusId { get; }
    }
}