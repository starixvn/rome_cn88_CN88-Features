namespace CN88.Features.CustomerList.Services
{
    using CN88.Features.Models;
    using CustomerList.Criteria;
    using DataAccesses;
    using Fanex.Data.Repository;
    using CustomerList.Models;
    using CustomerList.Params;
    using System.Collections.Generic;

    public interface CustomerListService
    {
        CustomerListModel GetCustomerList(CustomerListParam param);

        CreditRangeModel GetCreditRangeModel(int searchAccountId);

        Result UpdateCredit(UpdateCreditParam param);

        Result UpdateCustomerStatus(UpdateCustomerStatusParam param);

        IList<OnlineMemberModel> OnlineMembers(OnlineMembersParam param);
    }

    public class CustomerListServiceImpl : CustomerListService
    {
        private readonly int failErrorCode = 100;
        private readonly IDynamicRepository dynamicRepository;
        private readonly CustomerListDataAccess customerListDataAccess;

        public CustomerListServiceImpl(IDynamicRepository dynamicRepository, CustomerListDataAccess customerListDataAccess)
        {
            this.dynamicRepository = dynamicRepository;
            this.customerListDataAccess = customerListDataAccess;
        }

        public CustomerListModel GetCustomerList(CustomerListParam param)
        {
            var criteria = new CustomerListCriteria(param);

            return customerListDataAccess.GetCustomerList(criteria);
        }

        public CreditRangeModel GetCreditRangeModel(int searchAccountId)
            => dynamicRepository.Get<CreditRangeModel>(new SelectCreditRangeCriteria(searchAccountId));

        public Result UpdateCredit(UpdateCreditParam param)
        {
            var command = new UpdateCreditCriteria(
                    param.SearchAccountId,
                    param.CreditAmount,
                    param.ActorId,
                    param.SubAccountId,
                    param.Language,
                    param.IP);

            dynamicRepository.Execute(command);

            return command.ErrCode.Equals(0)
               ? Result.CreateErrorResult(failErrorCode, command.ErrMsg)
               : Result.CreateSuccessResult();
        }

        public Result UpdateCustomerStatus(UpdateCustomerStatusParam param)
        {
            var command = new UpdateCustomerStatusCriteria(
                    param.SearchAccountId,
                    param.ActorId,
                    param.SubAccountId,
                    param.UpdateStatusId,
                    param.Language,
                    param.IP);

            dynamicRepository.Execute(command);

            return command.ErrCode.Equals(0)
                ? Result.CreateSuccessResult() :
                Result.CreateErrorResult(command.ErrCode, command.ErrMsg);
        }

        public IList<OnlineMemberModel> OnlineMembers(OnlineMembersParam param)
        {
            var result = new List<OnlineMemberModel>
            {
                new OnlineMemberModel("Member 1","Name 1", new System.DateTime(2019,02,28)),
                new OnlineMemberModel("Member 2","Name 2", new System.DateTime(2019,02,28))
            };

            return result;
        }
    }
}
