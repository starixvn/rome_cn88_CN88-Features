namespace CN88.Features.Site.CustomerList.ViewModels
{
    using System.Collections.Generic;
    using Features.CustomerList.Models;

    public class OnlineMemberViewModel
    {
        public OnlineMemberViewModel(IList<OnlineMemberModel> members)
        {
            Members = members;
        }

        public IList<OnlineMemberModel> Members { get; }
    }
}