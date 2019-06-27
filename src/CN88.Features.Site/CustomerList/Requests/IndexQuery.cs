namespace CN88.Features.Site.CustomerList
{
    using Features.Models;
    using MediatR;
    using Site.Requests;
    using ViewModels;

    public class IndexQuery : BaseQuery, IRequest<IndexViewModel>
    {
        private const int DefaultPageSize = 100;

        private readonly IUser viewer;
        private const byte defaultStatusId = 4;
        private const byte defaultPageSize = 100;
        private const byte defaultPageIndex= 100;

        public IndexQuery()
            : this(null)
        {
        }

        public IndexQuery(IUser viewer)
            : base(viewer)
        {
            this.viewer = viewer;
        }

        public bool AccountManagementEditView => viewer.AccountManagementEditView;

        public int ActorId => viewer.Id;

        public string DownlineUsername { get; set; }

        public string Username { get; set; }

        public byte SearchRoleId { get; set; }

        public byte Status { get; set; } = defaultStatusId;

        public int PageSize { get; set; } = defaultPageSize;

        public int PageIndex { get; set; } = defaultPageIndex;
    }
}