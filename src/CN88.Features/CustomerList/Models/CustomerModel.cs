namespace CN88.Features.CustomerList.Models
{
    using System;
    using Extensions;

    public class CustomerModel
    {
        public CustomerModel()
        {
        }

        public CustomerModel(
            int custId,
            string username,
            string name,
            string nickname,
            string group,
            decimal credit,
            bool closed,
            DateTime creationDate,
            int status)
        {
            CustId = custId;
            Username = username;
            Name = name;
            Nickname = nickname;
            Group = group;
            Credit = credit;
            Closed = closed;
            CreationDate = creationDate;
            Status = status;
        }

        public int CustId { get; }

        public string Username { get; }

        public string Name { get; set; }

        public string Nickname { get; set; }

        public string Group { get; set; }

        public decimal Credit { get; }

        public bool Closed { get; }

        public DateTime CreationDate { get; }

        public string CreationDateStr => CreationDate.ToLongDateTime24h();

        public int Status { get; }
    }
}