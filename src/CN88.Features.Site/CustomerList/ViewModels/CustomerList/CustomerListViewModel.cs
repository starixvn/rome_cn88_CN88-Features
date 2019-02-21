namespace CN88.Features.Site.CustomerList.ViewModels
{
    using CN88.Features.CustomerList.Models;
    using System.Collections.Generic;
    using System.Web;

    public class CustomerListViewModel
    {
        public CustomerListViewModel(CustomerListModel model, int searchAccountId)
        {
            SearchAccountId = searchAccountId;
            TotalCustomers = model.TotalCustomers;
            Customers = model.Customers;

            foreach (var c in Customers)
            {
                c.Name = HttpUtility.HtmlEncode(c.Name);
                c.Group = HttpUtility.HtmlEncode(c.Group);
            }
        }
        public int SearchAccountId { get; }

        public int TotalCustomers { get; }

        public IEnumerable<CustomerModel> Customers { get; }
    }
}