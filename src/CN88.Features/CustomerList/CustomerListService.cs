namespace CN88.Features.CustomerList.Services
{
    using CN88.Features.Models;
    using CustomerList.Criteria;
    using CustomerList.Models;
    using CustomerList.Params;
    using DataAccesses;
    using Fanex.Data.Repository;
    using Resources;
    using Shared.Models;
    using System;
    using System.Collections.Generic;

    public interface CustomerListService
    {
        CustomerListModel GetCustomerList(CustomerListParam param);

        CreditRangeModel GetCreditRangeModel(int searchAccountId, bool isMember);

        Result UpdateCredit(UpdateCreditParam param);

        Result UpdateCustomerStatus(UpdateCustomerStatusParam param);

        IEnumerable<OnlineMemberModel> OnlineMembers(OnlineMembersParam param);
    }

    public class CustomerListServiceImpl : CustomerListService
    {      
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
            return new CreditRangeModel(
                credit.GivenCredit - memberUsedCredit.UsedCredit,
                credit.UsedCredit,
                memberUsedCredit.UsedCredit,
                credit.MaxCredit);         
        }

       

        public Result UpdateCredit(UpdateCreditParam param)
        {
            if (param.IsInternal)
            {
                return Result.CreateErrorResult(101, CustomerListResources.NotAuthorized);
            }

            var credit = GetCreditRangeModel(param.SearchAccountId, param.SearchAccountRoleId == UserLevel.Member.Value);
            if (param.CreditAmount < credit.MinCredit || param.CreditAmount > credit.MaxCredit)
            {
                return Result.CreateErrorResult(101, CustomerListResources.InvalidCredit);
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
               ? Result.CreateSuccessResult()
               : Result.CreateErrorResult(command.ErrCode, command.ErrMsg);
        }

        public Result UpdateCustomerStatus(UpdateCustomerStatusParam param)
        {
            if (param.IsInternal)
            {
                return Result.CreateErrorResult(101, CustomerListResources.NotAuthorized);
            }

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

        public IEnumerable<OnlineMemberModel> OnlineMembers(OnlineMembersParam param)
        {
            var criteria = new GetOnlineMemberListCriteria(param);
            var onlineMembers = dynamicRepository.Fetch<OnlineMemberModel>(criteria);
            param.TotalRow = criteria.TotalRows;

            return onlineMembers;
        }
            
    }
}
