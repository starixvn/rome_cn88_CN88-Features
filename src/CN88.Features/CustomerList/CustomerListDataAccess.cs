namespace CN88.Features.CustomerList.DataAccesses
{
    using CN88.Features.Models;
    using CustomerList.Criteria;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;
    using System.Collections.Generic;

    public interface CustomerListDataAccess
    {
        CustomerListModel GetCustomerList(CustomerListCriteria criteria);

        Result UpdateNonQuery(NonQueryCommand command);
    }

    public class CustomerListDataAccessImpl : CustomerListDataAccess
    {
        public CustomerListModel GetCustomerList(CustomerListCriteria criteria)
        {
            if (!criteria.IsValid())
            {
                return new CustomerListModel();
            }

            var totalCustomers = 0;
            var customers = new List<CustomerModel>();

            using (var objectDb = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = objectDb.ExecuteReader(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            customers.Add(new CustomerModel(
                                custId: Converter.ToInt(reader["CustId"]),
                                username: Converter.ToString2(reader["Username"]),
                                name: Converter.ToString2(reader["FirstName"]),
                                group: Converter.ToString2(reader["Group"]),
                                credit: Converter.ToDecimal(reader["Credit"]),
                                creationDate: Converter.ToDateTime(reader["CreatedDate"]),
                                status: Converter.ToInt(reader["Status"]),
                                closeReason: Converter.ToInt(reader["ClosedReason"])));
                        }

                        reader.Close();
                    }
                }

                totalCustomers = Converter.ToInt(objectDb.GetOuputParams()["totalrows"]);
            }

            return new CustomerListModel(totalCustomers, customers);
        }

        public Result UpdateNonQuery(NonQueryCommand command)
        {
            using (var objectDb = ObjectDbFactory.CreateInstance(command.GetSettingKey()))
            {
                objectDb.ExecuteNonQueryAsync(command.GetSpParams());

                var outputParams = objectDb.GetOuputParams();

                return new Result(
                    int.Parse(outputParams["errcode"].ToString()),
                    outputParams["errmsg"]?.ToString());
            }
        }
    }
}
