namespace CN88.Features.Reports.Services
{
    using Criteria;
    using Fanex.Data.Repository;
    using Features.Models;

    public interface ReCheckCustomerPasswordService
    {
        Result ReCheckPassword(int custId, int subAccountId, string userPassword);
    }

    public class ReCheckCustomerPasswordServiceImpl : ReCheckCustomerPasswordService
    {
        private readonly IDynamicRepository dynamicRepository;

        public ReCheckCustomerPasswordServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public Result ReCheckPassword(int custId, int subAccountId, string userPassword)
        {
            var command = new ReCheckCustomerPasswordCommand(custId, subAccountId, userPassword);

            dynamicRepository.Execute(command);

            return new Result(command.ErrCode, command.ErrMsg);
        }
    }
}