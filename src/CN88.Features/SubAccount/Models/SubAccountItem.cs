namespace CN88.Features.SubAccount.Models
{
    using System;
    using Extensions;
    using Sunplus.Agent.Model.Entities;

    public class SubAccountItem
    {
        public SubAccountItem()
        {
        }

        public SubAccountItem(
            int subAccId,
            string userName,
            string nickName,
            string firstName,
            DateTime createDate,
            string pageAccess,
            bool closed)
        {
            SubAccId = subAccId;
            UserName = userName;
            NickName = nickName;
            FirstName = firstName;
            CreateDate = createDate;
            PageAccess = pageAccess;
            Closed = closed;
        }

        public int SubAccId { get; set; }

        public string UserName { get; }

        public string NickName { get; }

        public string FirstName { get; }

        public DateTime CreateDate { get; }

        public string CreateDateString => CreateDate.ToLongDateTime24h();

        public string PageAccess { get; }

        public PageAccess Permissions => PageAccess;

        public bool Closed { get; }
    }
}