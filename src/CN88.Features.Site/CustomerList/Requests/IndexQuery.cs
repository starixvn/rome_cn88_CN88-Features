namespace CN88.Features.Site.CustomerList
{
    using CN88.Features.Models;
    using CN88.Features.Site.CustomerList.ViewModels;
    using CN88.Features.Site.Requests;
    using MediatR;

    public class IndexQuery : BaseQuery, IRequest<IndexViewModel>
    {
        private IUser viewer;

        public IndexQuery() : this(null)
        {

        }

        public IndexQuery(IUser viewer) : base(viewer)
        {
            this.viewer = viewer;
        }

        public bool IsInternal => viewer.IsInternal;

        public bool HasUpdatePermission => viewer.HasUpdatePermission;

        public bool HasAddNewPermission => viewer.HasAddNewPermission;

        public int ActorId => viewer.Id;

        public string DownlineUsername { get; set; }

        public string Username { get; set; }

        public byte SearchRoleId { get; set; }

        public byte Status { get; set; }

        public int PageSize { get; set; } = 100;

        public int PageIndex { get; set; } = 1;
    }
}