namespace CN88.Features.Site.CustomerList
{
    public class StatusPopupViewModel
    {
        public StatusPopupViewModel(CustomerStatusPopupQuery query)
        {
            CustId = query.CustId;
            UpdateStatusId = query.UpdateStatusId;
        }

        public int CustId { get; }

        public int UpdateStatusId { get; }
    }
}