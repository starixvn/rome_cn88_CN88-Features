namespace CN88.Features.CustomerList.Services
{
    using CN88.Features.Models;
    using CustomerList.Criteria;
    using DataAccesses;
    using Fanex.Data.Repository;
    using CustomerList.Models;
    using CustomerList.Params;
    using System.Collections.Generic;
    using Shared.Models;
    using Shared.AppResources;

    public interface CustomerListService
    {
        CustomerListModel GetCustomerList(CustomerListParam param);

        CreditRangeModel GetCreditRangeModel(int searchAccountId, bool isMember);

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

        public CreditRangeModel GetCreditRangeModel(int searchAccountId, bool isMember)
        {
            var credit = dynamicRepository.Get<CreditRangeModel>(new SelectCreditRangeCriteria(searchAccountId));
            if (!isMember)
            {
                return credit;
            }
            var memberUsedCredit = dynamicRepository.Get<CreditRangeModel>(new SelectMemberUsedCreditCriteria(searchAccountId));
            return new CreditRangeModel(memberUsedCredit.UsedCredit, credit);         
        }

       

        public Result UpdateCredit(UpdateCreditParam param)
        {
            var credit = GetCreditRangeModel(param.SearchAccountId, param.SearchAccountRoleId==UserLevel.Member.Value);
            if (param.CreditAmount < credit.MinCredit || param.CreditAmount > credit.MaxCredit)
            {
                return Result.CreateErrorResult(101, SharedResources.InvalidCredit);
            }

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
