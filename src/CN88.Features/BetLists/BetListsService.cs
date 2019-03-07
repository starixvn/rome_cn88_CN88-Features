namespace CN88.Features.BetLists
{
    using System.Collections.Generic;
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

        Task<BetListData> GetBetListOfStatement(BetListOfStatementParam param);

        Task<ParlayDetailReportModel> GetParlayDetails(GetParlayDetailParam param);

        Task<IEnumerable<TicketDto>> GetParlaySubTickets(GetParlaySubTicketsParam param);
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

        public async Task<BetListData> GetBetListOfStatement(BetListOfStatementParam param)
        {
            var criteria = new BetListOfStatementCriteria(param, reportDateService.GetMoveBetsDay());
            var betListDataTuple = await dynamicRepository.FetchMultipleAsync<TicketDto, TicketData>(criteria);

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }

        public async Task<ParlayDetailReportModel> GetParlayDetails(GetParlayDetailParam param)
        {
            var criteria = new GetParlayDetailCriteria(param, reportDateService.GetMoveBetsDay(), reportDateService.GetBalanceUpDay());

            var parlayDetailTuple = await dynamicRepository.FetchMultipleAsync<ParlayDetailModel, MatchDetailModel>(criteria);

            return new ParlayDetailReportModel(parlayDetailTuple.Item1, parlayDetailTuple.Item2, criteria.Outstanding);
        }

        public async Task<IEnumerable<TicketDto>> GetParlaySubTickets(GetParlaySubTicketsParam param)
        {
            var criteria = new GetParlaySubTicketsCriteria(param);
            var tickets = await dynamicRepository.FetchAsync<TicketDto>(criteria);

            return tickets;
        }
    }
}
