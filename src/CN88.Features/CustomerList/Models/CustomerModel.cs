namespace CN88.Features.CustomerList.Models
{
    using System;
    using Alpha.Utilities.Extensions;

    public class CustomerModel
    {
        public CustomerModel()
        {
        }

        public CustomerModel(
            int custId,
            string username,
            string name,
            string group,
            decimal credit,
            DateTime creationDate,
            int status,
            int closeReason)
        {
            CustId = custId;
            Username = username;
            Name = name;
            Group = group;
            Credit = credit;
            CreationDate = creationDate;
            Status = status;
            CloseReason = closeReason;
        }

        public int CustId { get; }

        public string Username { get; }

        public string Name { get; set; }

        public string Group { get; set; }

        public decimal Credit { get; }

        public DateTime CreationDate { get; }

        public string CreationDateStr => CreationDate.ToLongDateFormatWithSlash();

        public int Status { get; }

        public int CloseReason { get; }
    }
}
