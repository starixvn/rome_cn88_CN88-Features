namespace CN88.Features.SubAccount.Models
{
    using System;
    using Sunplus.Agent.Model.Entities;

    public class SubAccountItem
    {
        public SubAccountItem()
        {
        }

        public SubAccountItem(
            int subAccId,
            string userName,
            string firstName,
            string lastName,
            DateTime createdDate,
            bool closed,
            string pageAccess)
        {
            SubAccId = subAccId;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            CreatedDate = createdDate;
            Closed = closed;
            PageAccess = pageAccess;
        }

        public int SubAccId { get; }

        public string UserName { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public DateTime CreatedDate { get; }

        public bool Closed { get; }

        public string PageAccess { get; }

        public PageAccess Permission => PageAccess;
    }
}
