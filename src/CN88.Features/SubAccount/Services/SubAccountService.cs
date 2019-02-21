namespace CN88.Features.SubAccount.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Criteria;
    using Fanex.Data.Repository;
    using Models;

    public interface SubAccountService
    {
        Task<IEnumerable<SubAccountItem>> GetSubAccountsAsync(int custId);
    }

    public class SubAccountServiceImpl : SubAccountService
    {
        private readonly IDynamicRepository dynamicRepository;

        public SubAccountServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<SubAccountItem>> GetSubAccountsAsync(int custId)
            => await dynamicRepository.FetchAsync<SubAccountItem>(new GetSubAccountListCriteria(custId));
    }
}
