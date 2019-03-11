namespace CN88.Features.SubAccount.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Criteria;
    using Fanex.Data.Repository;
    using Features.Models;
    using Models;
    using Params;

    public interface SubAccountService
    {
        Task<IEnumerable<SubAccountItem>> GetSubAccountsAsync(int custId);

        Task<SubAccountItem> GetSubAccountAsync(int custId, int subAccId);

        Task<Result> CreateSubAccountAsync(CreateSubAccountParam param);

        Task<Result> UpdateSubAccountAsync(UpdateSubAccountParam param);

        Task<Result> UpdateSubAccountPermissionAsync(UpdateSubAccountPermissionParam param);
    }

    public class SubAccountServiceImpl : SubAccountService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly SubAccountValidator subAccountValidator;

        public SubAccountServiceImpl(
            IDynamicRepository dynamicRepository,
            SubAccountValidator subAccountValidator)
        {
            this.dynamicRepository = dynamicRepository;
            this.subAccountValidator = subAccountValidator;
        }

        public async Task<IEnumerable<SubAccountItem>> GetSubAccountsAsync(int custId)
            => await dynamicRepository.FetchAsync<SubAccountItem>(new GetSubAccountListCriteria(custId));

        public async Task<SubAccountItem> GetSubAccountAsync(int custId, int subAccId)
            => await dynamicRepository.GetAsync<SubAccountItem>(new GetSubAccountCriteria(custId, subAccId));

        public async Task<Result> CreateSubAccountAsync(CreateSubAccountParam param)
        {
            var result = subAccountValidator.ValidateEmpty(param.Password);

            if (!result.IsSuccess)
            {
                return result;
            }

            var rawPassword = subAccountValidator.Decrypt(param.Password, param.HashKey);
            result = subAccountValidator.IsMatchPasswordRules(rawPassword, param.InvalidPasswords);

            if (!result.IsSuccess)
            {
                return result;
            }

            param.Password = subAccountValidator.Encode(rawPassword);
            param.PageAccess = subAccountValidator.FilterWithValidPermissions(param.PageAccess);

            return await ExecuteAsync(param.ToCommand());
        }

        public async Task<Result> UpdateSubAccountAsync(UpdateSubAccountParam param)
            => await ExecuteAsync(param.ToCommand());

        public async Task<Result> UpdateSubAccountPermissionAsync(UpdateSubAccountPermissionParam param)
            => await ExecuteAsync(param.ToCommand());

        private async Task<Result> ExecuteAsync(CreateSubAccountCommand command)
        {
            await dynamicRepository.ExecuteAsync(command);

            return command.Success && command.SubAccId > 0
                ? Result.CreateSuccessResult()
                : Result.CreateErrorResult(command.ErrCode, command.ErrMsg);
        }

        private async Task<Result> ExecuteAsync(BaseSubAccountCommand command)
        {
            await dynamicRepository.ExecuteAsync(command);

            return new Result(command.ErrCode, command.ErrMsg);
        }
    }
}
