namespace CN88.Features.Site.CustomerList.ViewModels
{
    using System.Collections.Generic;
    using System.Web;
    using CN88.Features.CustomerList.Models;

    public class CustomerListViewModel
    {
        public CustomerListViewModel(
            CustomerListModel model,
            int searchAccountId,
            string searchAccountUsername,
            string downlineUsername)
        {
            SearchAccountId = searchAccountId;
            SearchAccountUsername = searchAccountUsername;
            DownlineUsername = downlineUsername;
            TotalCustomers = model.TotalCustomers;
            Customers = model.Customers;

            foreach (var customer in Customers)
            {
                customer.Name = HttpUtility.HtmlEncode(customer.Name);
                customer.Group = HttpUtility.HtmlEncode(customer.Group);
            }
        }

        public int SearchAccountId { get; }

        public string SearchAccountUsername { get; }

        public string DownlineUsername { get; }

        public int TotalCustomers { get; }

        public IEnumerable<CustomerModel> Customers { get; }
    }
}