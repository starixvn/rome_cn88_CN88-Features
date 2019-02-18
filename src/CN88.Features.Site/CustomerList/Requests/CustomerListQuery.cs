namespace CN88.Features.Site.CustomerList
{
    using CN88.Features.Models;
    using CN88.Features.Shared.Models;
    using CN88.Features.Site.CustomerList.ViewModels;
    using CN88.Features.Site.Requests;
    using MediatR;
    using System.Web.Mvc;

    public class CustomerListQuery : IRequest<CustomerListViewModel>
    {
        public CustomerListQuery()
        {

        }
        //public CustomerListQuery(IUser viewer) : base(viewer)
        //{
        //}

        [AllowHtml]
        public string DownlineUsername { get; set; }

        [AllowHtml]
        public string Username { get; set; }

        public int SearchRoleId { get; set; }

        public int Status { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }


        public int CustId { get; set; }

        public string CustName { get; set; }

        public byte CustLevelId { get; set; }

        public virtual UserLevel CustLevel => Enumeration.FromValue<UserLevel>(CustLevelId);

        public int ViewerId => 44;

        public string ViewerName => "test";

        public byte ViewerLevelId => 4;

        public virtual UserLevel ActorLevel => Enumeration.FromValue<UserLevel>(ViewerLevelId);

        public int SubAccountId => 0;

        public string IP => string.Empty;

        public string Language => "en";
    }
}