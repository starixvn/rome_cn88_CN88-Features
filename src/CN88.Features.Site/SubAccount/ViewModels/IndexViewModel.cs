namespace CN88.Features.Site.SubAccount.ViewModels
{
    using System;
    using Features.SubAccount.Models;
    using Sunplus.Agent.Model.Entities;

    public class IndexViewModel
    {
        public IndexViewModel(SubAccountItem subAccount)
        {
            SubAccId = subAccount.SubAccId;
            UserName = subAccount.UserName;
            FirstName = subAccount.FirstName;
            LastName = subAccount.LastName;
            CreatedDate = subAccount.CreatedDate;
            Closed = subAccount.Closed;
            PageAccess = subAccount.PageAccess;
        }

        public int SubAccId { get; }

        public string UserName { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public DateTime CreatedDate { get; }

        public bool Closed { get; }

        public PageAccess PageAccess { get; }
    }
}