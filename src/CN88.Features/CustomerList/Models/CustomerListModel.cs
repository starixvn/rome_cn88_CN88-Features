namespace CN88.Features.CustomerList.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class CustomerListModel
    {
        public CustomerListModel()
        {
            TotalCustomers = 0;
            Customers = Enumerable.Empty<CustomerModel>();
        }

        public CustomerListModel(
            int totalCustomers, 
            IEnumerable<CustomerModel> customers, 
            int uplineId,
            byte uplineStatus)
        {
            TotalCustomers = totalCustomers;
            Customers = customers;
            UplineId = uplineId;
            UplineStatus = uplineStatus;
        }

        public int UplineId { get; }

        public byte UplineStatus { get; }

        public int TotalCustomers { get; }

        public IEnumerable<CustomerModel> Customers { get; }
    }
}