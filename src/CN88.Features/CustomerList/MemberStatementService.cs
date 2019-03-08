namespace CN88.Features.CustomerList
{
    using System;
    using System.Collections.Generic;
    using Alpha.Utilities.Extensions;
    using CN88.Features.CustomerList.Criteria;
    using CN88.Features.CustomerList.Models;
    using CN88.Features.CustomerList.Params;
    using CN88.Features.Shared.Services;
    using Fanex.Caching;
    using Fanex.Data.Repository;
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
            var fromDate = GetAvailableDate()[param.FromDate];
            var toDate = GetAvailableDate()[param.ToDate];
            var criteria = new GetMemberStatementCriteria(
                param.CustId,
                param.SportType,
                fromDate,
                toDate,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay());

            return dynamicRepository.Fetch<MemberStatementModel>(criteria);
        }

        public Dictionary<int, DateTime> GetAvailableDate()
        {
            return ConfiguredCacheService.GetOrSet(
                "CN88.Features.CustomerList.GetAvailableDate",
                () =>
                {
                    var dates = new Dictionary<int, DateTime>();
                    var numberLatestDay = 8;
                    for (int i = 0; i < numberLatestDay; i++)
                    {
                        dates.Add(i, SystemTime.Today.AddDays(i * -1));
                    }

                    return dates;
                },
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));
        }
    }
}
