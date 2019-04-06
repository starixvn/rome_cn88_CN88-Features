namespace CN88.Features.CustomerList
{
    using System;
    using System.Collections.Generic;
    using CN88.Features.CustomerList.Criteria;
    using CN88.Features.CustomerList.Models;
    using CN88.Features.CustomerList.Params;
    using CN88.Features.Shared.Services;
    using Fanex.Data.Repository;
    using Shared.Configurations;
    using Shared.Utils;

    public interface MemberStatementService
    {
        IEnumerable<MemberStatementModel> Get(MemberStatementParam param);

        Dictionary<int, DateTime> GetAvailableDate();
    }

    public class MemberStatementServiceImpl : MemberStatementService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly ReportDateService reportDateService;

        public MemberStatementServiceImpl(
            IDynamicRepository dynamicRepository,
            ReportDateService reportDateService)
        {
            this.dynamicRepository = dynamicRepository;
            this.reportDateService = reportDateService;
        }

        public IEnumerable<MemberStatementModel> Get(MemberStatementParam param)
        {
            var availableDates = GetAvailableDate();
            var fromDate = availableDates[param.FromDate];
            var toDate = availableDates[param.ToDate];

            var criteria = new GetMemberStatementCriteria(
                param.CustId,
                param.SportType,
                fromDate,
                toDate,
                reportDateService.GetMoveBetsDay(),
                reportDateService.GetBalanceUpDay());

            return dynamicRepository.Fetch<MemberStatementModel>(criteria);
        }

        public Dictionary<int, DateTime> GetAvailableDate()
        {
            var dates = new Dictionary<int, DateTime>();

            for (var i = 0; i < AppSettings.MemberStatementOfLatestDay; i++)
            {
                dates.Add(i, SystemTime.Today.AddDays(i * -1));
            }

            return dates;
        }
    }
}
