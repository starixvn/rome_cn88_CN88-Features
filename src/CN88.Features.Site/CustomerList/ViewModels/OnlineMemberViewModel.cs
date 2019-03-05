namespace CN88.Features.Site.CustomerList.ViewModels
{
    using System.Collections.Generic;
    using Features.CustomerList.Models;

    public class OnlineMemberViewModel
    {
        public OnlineMemberViewModel(IEnumerable<OnlineMemberModel> members)
        {
            Members = members;
        }

        public IEnumerable<OnlineMemberModel> Members { get; }
    }
}