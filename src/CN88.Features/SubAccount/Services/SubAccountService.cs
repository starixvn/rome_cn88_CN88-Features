namespace CN88.Features.SubAccount.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AppResources;
    using Constants;
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
        {
            var subAccount = await dynamicRepository.GetAsync<SubAccountItem>(new GetSubAccountCriteria(custId, subAccId));

            if (subAccount != null)
            {
                subAccount.SubAccId = subAccId;
            }

            return subAccount;
        }

        public async Task<Result> CreateSubAccountAsync(CreateSubAccountParam param)
        {
            var result = ValidateCreateSubAccountParam(param);

            if (result.IsSuccess)
            {
                param.UserName = $"{param.ActorName}{SubAccountConst.SubAccountPadding}{param.Suffix1}{param.Suffix2}";
                param.Password = subAccountValidator.Encode(param.Password);
                param.PageAccess = subAccountValidator.FilterWithValidPermissions(param.PageAccess);

                result = await ExecuteAsync(new CreateSubAccountCommand(param));
            }

            return result;
        }

        public async Task<Result> UpdateSubAccountAsync(UpdateSubAccountParam param)
        {
            var result = subAccountValidator.ValidateEmptyName(param.Name);

            if (!result.IsSuccess)
            {
                return result;
            }

            var subAccount = await GetSubAccountAsync(param.ActorId, param.SubAccId);

            return subAccount != null
                ? await UpdateSubAccountAndKickoutAsync(param, subAccount.PageAccess)
                : Result.CreateSuccessResult(message: SubAccountResource.UpdateSuccessfully);
        }

        private Result ValidateCreateSubAccountParam(CreateSubAccountParam param)
        {
            var result = subAccountValidator.ValidateEmptyName(param.Name);

            if (!result.IsSuccess)
            {
                return result;
            }

            result = subAccountValidator.ValidateEmptyPassword(param.Password);

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

            result = subAccountValidator.ValidateSuffix(param.Suffix1, param.Suffix1);

            if (!result.IsSuccess)
            {
                return result;
            }

            param.Password = rawPassword;
            return Result.CreateSuccessResult();
        }

        private async Task<Result> UpdateSubAccountAndKickoutAsync(UpdateSubAccountParam param, string oldPageAccess)
        {
            param.PageAccess = subAccountValidator.FilterWithValidPermissions(param.PageAccess);
            var result = await ExecuteAsync(new UpdateSubAccountCommand(param));

            // Kickout when updating Permissions.
            if (result.IsSuccess && oldPageAccess != param.PageAccess)
            {
                await dynamicRepository.ExecuteAsync(
                    new KickoutSubAccountCommand(param.ActorId, param.SubAccId));
            }

            return result;
        }

        private async Task<Result> ExecuteAsync(BaseSubAccountCommand command)
        {
            await dynamicRepository.ExecuteAsync(command);

            return new Result(
                command.ErrCode,
                command.ErrCode == 0
                ? (command is CreateSubAccountCommand
                    ? SubAccountResource.AccountSuccessfullyCreated
                    : SubAccountResource.UpdateSuccessfully)
                : command.ErrMsg);
        }
    }
}
