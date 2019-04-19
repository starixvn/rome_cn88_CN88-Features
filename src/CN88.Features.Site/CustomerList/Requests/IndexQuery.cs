namespace CN88.Features.Site.CustomerList
{
    using Features.Models;
    using MediatR;
    using Site.Requests;
    using ViewModels;

    public class IndexQuery : BaseQuery, IRequest<IndexViewModel>
    {
        private readonly IUser viewer;

        public IndexQuery()
            : this(null)
        {
        }

        public IndexQuery(IUser viewer)
            : base(viewer)
        {
            this.viewer = viewer;
        }

        public bool IsInternal => viewer.IsInternal;

        public bool AccountManagementEditView => viewer.AccountManagementEditView;

        public int ActorId => viewer.Id;

        public string DownlineUsername { get; set; }

        public string Username { get; set; }

        public byte SearchRoleId { get; set; }

        public byte Status { get; set; }

        public int PageSize { get; set; } = 100;

        public int PageIndex { get; set; } = 1;
    }
}