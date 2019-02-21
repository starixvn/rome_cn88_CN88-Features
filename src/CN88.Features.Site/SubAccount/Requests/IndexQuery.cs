namespace CN88.Features.Site.SubAccount.Requests
{
    using System.Collections.Generic;
    using Features.Models;
    using MediatR;
    using Site.Requests;
    using ViewModels;

    public class IndexQuery : BaseQuery, IRequest<IEnumerable<IndexViewModel>>
    {
        public IndexQuery(IUser viewer)
            : base(viewer)
        {
        }
    }
}