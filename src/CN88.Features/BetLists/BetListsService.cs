namespace CN88.Features.BetLists
{
    using System.Collections.Generic;
    using System.Linq;
    using Criteria;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data.Repository;
    using Models;
    using Params;
    using Shared.Services;

    public interface BetListsService
    {
        BetListData GetBetListOfReport(BetListOfReportParam param);

        BetListData GetBetListOfStatement(BetListOfStatementParam param);

        ParlayDetailReportModel GetParlayDetails(GetParlayDetailParam param);

        IEnumerable<TicketDto> GetParlaySubTickets(GetParlaySubTicketsParam param);
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

        public BetListData GetBetListOfReport(BetListOfReportParam param)
        {
            var criteria = BetListOfReportCriteriaFactory.CreateCriteria(param, reportDateService.GetMoveBetsDay());
            var betListDataTuple = dynamicRepository.FetchMultiple<TicketDto, TicketData>(criteria);

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }

        public BetListData GetBetListOfStatement(BetListOfStatementParam param)
        {
            var criteria = new BetListOfStatementCriteria(param, reportDateService.GetMoveBetsDay());
            var betListDataTuple = dynamicRepository.FetchMultiple<TicketDto, TicketData>(criteria);

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }

        public ParlayDetailReportModel GetParlayDetails(GetParlayDetailParam param)
        {
            var criteria = new GetParlayDetailCriteria(param, reportDateService.GetMoveBetsDay(), reportDateService.GetBalanceUpDay());

            var parlayDetailTuple = dynamicRepository.FetchMultiple<ParlayDetailModel, MatchDetailModel>(criteria);

            return new ParlayDetailReportModel(parlayDetailTuple.Item1, parlayDetailTuple.Item2, criteria.Outstanding);
        }

        public IEnumerable<TicketDto> GetParlaySubTickets(GetParlaySubTicketsParam param)
        {
            var criteria = new GetParlaySubTicketsCriteria(param);
            var tickets = dynamicRepository.Fetch<TicketDto>(criteria);

            return tickets;
        }
    }
}