namespace CN88.Features.CustomerList.DataAccesses
{
    using System.Collections.Generic;
    using CustomerList.Criteria;
    using Fanex.Common;
    using Fanex.Data;
    using Models;

    public interface CustomerListDataAccess
    {
        CustomerListModel GetCustomerList(CustomerListCriteria criteria);
    }

    public class CustomerListDataAccessImpl : CustomerListDataAccess
    {
        public CustomerListModel GetCustomerList(CustomerListCriteria criteria)
        {
            using (var objectDb = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                IEnumerable<CustomerModel> downlines;
                using (var resultReader = objectDb.QueryMultiple(criteria.GetSpParams()))
                {
                     downlines = resultReader.Read<CustomerModel>();
                }

                var outputParams = objectDb.GetOuputParams();

                return new CustomerListModel(
                    Converter.ToInt(outputParams["totalrows"]), 
                    downlines, 
                    Converter.ToInt(outputParams["uplineid"]),
                    outputParams["uplineStatus"] == null ? (byte)4 : (byte)outputParams["uplineStatus"]);
            }
        }
    }
}