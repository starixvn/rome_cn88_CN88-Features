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

            foreach (var c in Customers)
            {
                c.Name = HttpUtility.HtmlEncode(c.Name);
                c.Group = HttpUtility.HtmlEncode(c.Group);
            }
        }

        public int SearchAccountId { get; }

        public string SearchAccountUsername { get; }

        public string DownlineUsername { get; }

        public int TotalCustomers { get; }

        public IEnumerable<CustomerModel> Customers { get; }
    }
}