namespace CN88.Features.CustomerList.Models
{
    using System;
    using System.Web;
    using Extensions;

    public class CustomerModel
    {
        private string nickname;
        private string name;
        private string group;

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

        public string Name
        {
            get { return HttpUtility.HtmlEncode(name); }

            set { name = value; }
        }

        public string Nickname
        {
            get { return nickname ?? string.Empty; }

            set { nickname = value; }
        }

        public string Group
        {
            get { return HttpUtility.HtmlEncode(group); }

            set { group = value; }
        }

        public decimal Credit { get; }

        public bool Closed { get; }

        public DateTime CreationDate { get; }

        public string CreationDateStr => CreationDate.ToLongDateTime24h();

        public int Status { get; }
    }
}