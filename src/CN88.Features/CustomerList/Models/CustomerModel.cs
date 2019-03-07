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
            DateTime creationDate,
            int status,
            int closeReason)
        {
            CustId = custId;
            Username = username;
            Name = name;
            Nickname = nickname;
            Group = group;
            Credit = credit;
            CreationDate = creationDate;
            Status = status;
            CloseReason = closeReason;
        }

        public int CustId { get; }

        public string Username { get; }

        public string Name { get; set; }

        public string Nickname { get; set; }

        public string Group { get; set; }

        public decimal Credit { get; }

        public DateTime CreationDate { get; }

        public string CreationDateStr => CreationDate.ToLongDateTime24h();

        public int Status { get; }

        public int CloseReason { get; }
    }
}
