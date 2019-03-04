namespace CN88.Features.BetLists
{
    using System.Linq;
    using System.Threading.Tasks;
    using Criteria;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;
    using Models;
    using Params;
    using Shared.Services;

    public interface BetListsService
    {
        Task<BetListData> GetBetListOfReport(BetListOfReportParam param);
    }

    public class BetListsServiceImpl : BetListsService
    {
        private readonly ReportDateService reportDateService;
        private readonly IDynamicRepository dynamicRepository;

        public BetListsServiceImpl(
            ReportDateService reportDateService,
            IDynamicRepository dynamicRepository)
        {
            this.reportDateService = reportDateService;
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<BetListData> GetBetListOfReport(BetListOfReportParam param)
        {
            var criteria = new BetListOfReportCriteria(param, reportDateService.GetMoveBetsDay());
            var betListDataTuple = await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }
    }
}
