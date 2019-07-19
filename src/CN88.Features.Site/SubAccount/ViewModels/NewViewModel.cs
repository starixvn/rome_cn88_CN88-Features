namespace CN88.Features.Site.SubAccount.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using CN88.Features.SubAccount.Models;

    public class NewViewModel
    {
        public NewViewModel(
            string subAccountName,
            string hashKey,
            bool showAccountManagementPermission,
            Tuple<bool, IEnumerable<ManagedAccount>> managedAccounts)
        {
            SubAccountName = subAccountName;
            HashKey = hashKey;
            ShowAccountManagementPermission = showAccountManagementPermission;
            ManagedAccountViewModel = new ManagedAccountViewModel(managedAccounts.Item1, managedAccounts.Item2);
        }

        public string SubAccountName { get; }

        public string HashKey { get; }

        public bool ShowAccountManagementPermission { get; }

        public ManagedAccountViewModel ManagedAccountViewModel { get; }

        public IEnumerable<SelectListItem> Suffix1
            => new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" }
            .Select(item => new SelectListItem { Value = item, Text = item, Selected = item == "0" });

        public IEnumerable<SelectListItem> Suffix2
            => new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" }
            .Select(item => new SelectListItem { Value = item, Text = item, Selected = item == "1" });
    }
}