namespace CN88.Features.Site.SubAccount.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CN88.Features.SubAccount.Models;

    public class EditViewModel
    {
        public EditViewModel(
            SubAccountItem subAccountItem,
            Tuple<bool, IEnumerable<ManagedAccount>> managedAccounts)
        {
            SubAccountItem = subAccountItem;
            NumberOfDownline = managedAccounts.Item2.Count();
            ManagedAccountViewModel = new ManagedAccountViewModel(managedAccounts.Item1, managedAccounts.Item2);
        }

        public SubAccountItem SubAccountItem { get; }

        public int NumberOfDownline { get; }

        public ManagedAccountViewModel ManagedAccountViewModel { get; }
    }
}