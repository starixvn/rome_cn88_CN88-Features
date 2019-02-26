namespace CN88.Features.Site.SubAccount.ViewModels
{
    using System.Collections.Generic;
    using Features.SubAccount.Models;

    public class IndexViewModel
    {
        public IndexViewModel(
            string userCode,
            int pageIndex,
            int pageSize,
            int totalItems,
            IEnumerable<SubAccountItem> subAccounts)
        {
            UserCode = userCode;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalItems = totalItems;
            SubAccounts = subAccounts;
        }

        public string UserCode { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalItems { get; }

        public IEnumerable<SubAccountItem> SubAccounts { get; }
    }
}