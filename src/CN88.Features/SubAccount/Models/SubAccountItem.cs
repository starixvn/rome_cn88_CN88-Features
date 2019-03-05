namespace CN88.Features.SubAccount.Models
{
    using System;
    using Alpha.Utilities.Extensions;
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
            DateTime createDate,
            bool closed,
            string pageAccess)
        {
            SubAccId = subAccId;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            CreateDate = createDate;
            Closed = closed;
            PageAccess = pageAccess;
        }

        public int SubAccId { get; }

        public string UserName { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public DateTime CreateDate { get; }

        public string CreateDateString => CreateDate.ToLongDateFormatWithSlash();

        public bool Closed { get; }

        public string PageAccess { get; }

        public PageAccess Permissions => PageAccess;
    }
}
