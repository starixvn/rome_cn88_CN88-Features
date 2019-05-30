namespace CN88.Features.BetLists
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CN88.Features.BetLists.Criteria.BetListOfReport;
    using CN88.Features.BetLists.Criteria.Parlay;
    using CN88.Features.BetLists.Criteria.TotalBets;
    using Criteria;
    using Extensions;
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

        BetListData GetOutrightWager(OutrightWagerParam param);

        BetListData GetEventWager(EventWagerParam param);

        BetListData GetTotalBetParlay(GetTotalBetParlayParam param);

        BetListData GetStartedWager(StartedWagerParam param);
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
            var balanceUpDate = reportDateService.GetBalanceUpDay();
            var previousBalanceUpDate = balanceUpDate.AddDays(-1);
            Tuple<IEnumerable<TicketDto>, IEnumerable<TicketData>> betListDataTuple;
            var fromDate = param.FromDate.GetGreaterDate(param.LimitedDate);
            if (fromDate.IsBefore(balanceUpDate))
            {
                if (param.ToDate.IsBeforeOrEqual(previousBalanceUpDate))
                {
                    betListDataTuple = GetBetListBeforeBalanceUpDay(param, fromDate: fromDate, toDate: param.ToDate);
                }
                else
                {
                    betListDataTuple = GetBetList(param, fromDate: balanceUpDate);
                    var oldBetListDataTuple = GetBetListBeforeBalanceUpDay(param, fromDate: fromDate, toDate: previousBalanceUpDate);

                    return new BetListData(
                        betListDataTuple.Item1.Concat(oldBetListDataTuple.Item1),
                        betListDataTuple.Item2.Concat(oldBetListDataTuple.Item2).ToList<ITicketData>());
                }
            }
            else
            {
                betListDataTuple = GetBetList(param, fromDate);
            }

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }

        public BetListData GetBetListOfStatement(BetListOfStatementParam param)
            => GetBetListData(new BetListOfStatementCriteria(param, reportDateService.GetMoveBetsDay()));

        public BetListData GetOutrightWager(OutrightWagerParam param)
            => GetBetListData(new OutrightWagerCriteria(param));

        public BetListData GetEventWager(EventWagerParam param)
            => GetBetListData(new EventWagerCriteria(param));

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

        public BetListData GetTotalBetParlay(GetTotalBetParlayParam param)
            => GetBetListData(new TotalBetParlayCriteria(param, reportDateService.GetMoveBetsDay()));

        public BetListData GetStartedWager(StartedWagerParam param)
            => GetBetListData(new StartedWagerCriteria(param));

        private Tuple<IEnumerable<TicketDto>, IEnumerable<TicketData>> GetBetListBeforeBalanceUpDay(BetListOfReportParam param, DateTime fromDate, DateTime toDate)
          => dynamicRepository.FetchMultiple<TicketDto, TicketData>(
              BetListOfReportCriteriaFactory.CreateCriteria(param, reportDateService.GetMoveBetsDay(), fromDate, toDate, isBackup: true));

        private Tuple<IEnumerable<TicketDto>, IEnumerable<TicketData>> GetBetList(BetListOfReportParam param, DateTime fromDate)
            => dynamicRepository.FetchMultiple<TicketDto, TicketData>(
                BetListOfReportCriteriaFactory.CreateCriteria(param, reportDateService.GetMoveBetsDay(), fromDate, param.ToDate, isBackup: false));

        private BetListData GetBetListData(CriteriaBase criteria)
        {
            var betListDataTuple = dynamicRepository.FetchMultiple<TicketDto, TicketData>(criteria);

            return new BetListData(betListDataTuple.Item1, betListDataTuple.Item2.ToList<ITicketData>());
        }
    }
}