namespace CN88.Features.BetLists
{
    using System.Collections.Generic;
    using Models;
    using Params;

    public interface BetListsService
    {
        IList<DailyWinLossModel> GetDailyWinLoss(DailyWinLossParam param);
    }

    public class BetListsServiceImpl : BetListsService
    {
        public IList<DailyWinLossModel> GetDailyWinLoss(DailyWinLossParam param)
        {
            var result = new List<DailyWinLossModel>
            {
                new DailyWinLossModel(
                    new System.DateTime(2019,04,02,13,42,34),
                    "aRS21234",
                    (decimal)2.25,
                    "HK 11004722724",
                    1,
                    "Indonesia Cup",
                    50,
                    (decimal)-48.9,
                    2,
                    40,
                    30,
                    0,
                    30)
            };

            return result;
        }
    }
}
