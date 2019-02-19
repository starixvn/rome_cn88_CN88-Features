namespace CN88.Features.Site.CustomerList
{
    using CN88.Features.Models;
    using CN88.Features.Shared.Models;
    using CN88.Features.Site.CustomerList.ViewModels;
    using CN88.Features.Site.Requests;
    using MediatR;
    using System.Web.Mvc;

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
    }
}