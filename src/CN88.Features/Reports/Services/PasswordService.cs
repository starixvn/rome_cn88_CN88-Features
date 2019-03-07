namespace CN88.Features.Reports.Services
{
    using Criteria;
    using Fanex.Data.Repository;
    using Features.Models;

    public interface PasswordService
    {
        Result CheckCustomerPassword(int custId, int subAccountId, string userPassword);
    }

    public class PasswordServiceImpl : PasswordService
    {
        private readonly IDynamicRepository dynamicRepository;

        public PasswordServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public Result CheckCustomerPassword(int custId, int subAccountId, string userPassword)
        {
            var command = new ReCheckCustomerPasswordCommand(custId, subAccountId, userPassword);

            if (command.IsInvalid())
            {
                return Result.CreateErrorResult();
            }

            dynamicRepository.Execute(command);

            return new Result(command.ErrCode, command.ErrMsg);
        }
    }
}