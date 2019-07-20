namespace CN88.Features.Site.SubAccount.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using CN88.Features.Shared.Extensions;
    using CN88.Features.SubAccount.Models;

    public class ManagedAccountViewModel
    {
        public ManagedAccountViewModel(
            bool isAllDownline,
            IEnumerable<ManagedAccount> managedAccounts)
        {
            IsAllDownline = isAllDownline;
            ManagedAccounts = managedAccounts;
        }

        public bool IsAllDownline { get; }

        public IEnumerable<ManagedAccount> ManagedAccounts { get; }

        public string DownlineId => ManagedAccounts.Where(x => x.Assigned).JoinWithComma();
    }
}