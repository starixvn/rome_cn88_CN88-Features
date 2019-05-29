namespace CN88.Features.Site.CustomerList.ViewModels
{
    using CN88.Features.CustomerList.Models;
    using System.Collections.Generic;

    public class CustomerListViewModel
    {
        public CustomerListViewModel(
            CustomerListModel model,
            int searchAccountId,
            string searchAccountUsername,
            byte searchAccountStatus,
            string downlineUsername)
        {
            SearchAccountId = searchAccountId;
            SearchAccountUsername = searchAccountUsername;
            SearchAccountStatus = searchAccountStatus;
            DownlineUsername = downlineUsername;
            TotalCustomers = model.TotalCustomers;
            Customers = model.Customers;
        }

        public int SearchAccountId { get; }

        public string SearchAccountUsername { get; }

        public byte SearchAccountStatus { get; }

        public string DownlineUsername { get; }

        public int TotalCustomers { get; }

        public IEnumerable<CustomerModel> Customers { get; }
    }
}