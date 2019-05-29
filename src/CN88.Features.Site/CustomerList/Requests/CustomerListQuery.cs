namespace CN88.Features.Site.CustomerList
{
    using System.Web.Mvc;
    using CN88.Features.CustomerList;
    using CN88.Features.Models;
    using CN88.Features.Site.CustomerList.ViewModels;
    using CN88.Features.Site.Requests;
    using MediatR;

    public class CustomerListQuery : BaseQuery, IRequest<CustomerListViewModel>
    {
        public CustomerListQuery() : this(null)
        {
        }

        public CustomerListQuery(IUser viewer) : base(viewer)
        {
        }

        [AllowHtml]
        public string DownlineUsername { get; set; }

        [AllowHtml]
        public string Username { get; set; }

        public int SearchRoleId { get; set; }

        public int Status { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public override bool IsValid() => base.IsValid() && ViewerStatusId.Equals(CustomerStatus.Active);
    }
}