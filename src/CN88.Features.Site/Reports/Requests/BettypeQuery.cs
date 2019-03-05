namespace CN88.Features.Site.Reports.Requests
{
    using Features.Models;
    using MediatR;
    using Site.Requests;
    using ViewModels;

    public class BettypeQuery : BaseQuery, IRequest<BettypeViewModel>
    {
        public BettypeQuery(IUser viewer)
            : base(viewer)
        {
        }

        public int SportType { get; set; }
    }
}