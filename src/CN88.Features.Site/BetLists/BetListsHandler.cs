namespace CN88.Features.Site.BetLists
{
    using Features.BetLists;
    using Features.BetLists.Params;
    using MediatR;
    using Requests;
    using ViewModels;

    public class BetListsHandler :
         IRequestHandler<DailyWinLossQuery, DailyWinLossViewModel>
    {
        private BetListsService service;

        public BetListsHandler(BetListsService service)
        {
            this.service = service;
        }

        public DailyWinLossViewModel Handle(DailyWinLossQuery message)
        {
            var winLoss = service.GetDailyWinLoss(new DailyWinLossParam());

            return new DailyWinLossViewModel(message, winLoss);
        }
    }
}