namespace CN88.Features.Site.CustomerList
{
    using CN88.Features.Models;
    using CN88.Features.Site.CustomerList.ViewModels;
    using CN88.Features.Site.Requests;
    using MediatR;

    public class IndexQuery : IRequest<IndexViewModel>
    {
        public IndexQuery()
        {
            IsInternal = false;
            HasUpdatePermission = true;
            HasAddNewPermission = true;
        }

        //public IndexQuery(IUser viewer) : base(viewer)
        //{
        //    IsInternal = viewer.IsInternal;
        //    HasUpdatePermission = viewer.HasUpdatePermission;
        //    HasAddNewPermission = viewer.HasAddNewPermission;
        //}

        public bool IsInternal { get; }

        public bool HasUpdatePermission { get; }

        public bool HasAddNewPermission { get; }

        public string DownlineUsername { get; set; }

        public string Username { get; set; }

        public byte SearchRoleId { get; set; }

        public byte Status { get; set; }

        public int PageSize { get; set; } = 100;

        public int PageIndex { get; set; } = 1;
    }
}