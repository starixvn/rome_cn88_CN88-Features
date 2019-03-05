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

        TicketDetailModel GetTicketDetail(GetTicketDetailParam param);

        Task<BetListData> GetSubTickets(GetSubTicketsParam param);
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

        public TicketDetailModel GetTicketDetail(GetTicketDetailParam param)
        {
            var criteria = new GetTicketDetailCriteria(param, reportDateService.GetMoveBetsDay(), reportDateService.GetBalanceUpDay());

            return new TicketDetailModel(null);
        }

        public async Task<BetListData> GetSubTickets(GetSubTicketsParam param)
        {
            var criteria = new GetSubTicketsCriteria(param);
            var betListDataTuple = await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }
    }
}
