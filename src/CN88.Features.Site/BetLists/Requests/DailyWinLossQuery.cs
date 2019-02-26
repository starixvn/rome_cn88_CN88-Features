namespace CN88.Features.Site.BetLists.Requests
{
    using Features.Models;
    using MediatR;
    using Site.Requests;
    using ViewModels;

    public class DailyWinLossQuery : BaseQuery, IRequest<DailyWinLossViewModel>
    {
        public DailyWinLossQuery(IUser viewer) : base(viewer)
        {
        }
    }
}