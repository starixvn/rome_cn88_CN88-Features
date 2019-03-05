namespace CN88.Features.Site.CustomerList.ViewModels
{
    using System.Collections.Generic;
    using Features.CustomerList.Models;
    using Requests;
    using Shared.ViewModels.Components;

    public class OnlineMemberViewModel
    {
        public OnlineMemberViewModel(
            IEnumerable<OnlineMemberModel> members,
            OnlineMembersQuery query,
            int totalRows)
        {
            Members = members;
            this.query = query;
            this.totalRows = totalRows;
        }

        private readonly int totalRows;
        private readonly OnlineMembersQuery query;

        public IEnumerable<OnlineMemberModel> Members { get; }

        public PagerViewModel PagerViewModel =>
           new PagerViewModel(query.PageIndex, query.PageSize, totalRows);
    }
}