namespace CN88.Features.CustomerList
{
    using System.Collections.Generic;
    using CN88.Features.CustomerList.Criteria;
    using CN88.Features.CustomerList.Models;
    using CN88.Features.CustomerList.Params;
    using CN88.Features.Shared.Services;
    using Fanex.Data.Repository;

    public interface MemberStatementService
    {
        IEnumerable<MemberStatementModel> Get(MemberStatementParam param);
    }

    public class MemberStatementServiceImpl: MemberStatementService
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
            var criteria = new GetMemberStatementCriteria(
                param.CustId,
                param.SportType,
                param.FromDate,
                param.ToDate,
                reportDateService.GetBalanceUpDay(),
                reportDateService.GetMoveBetsDay());

            return dynamicRepository.Fetch<MemberStatementModel>(criteria);
        }
    }
}
