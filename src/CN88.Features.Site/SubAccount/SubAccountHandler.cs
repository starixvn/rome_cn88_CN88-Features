namespace CN88.Features.Site.SubAccount
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Features.SubAccount.Services;
    using MediatR;
    using Requests;
    using ViewModels;

    public class SubAccountHandler :
        IAsyncRequestHandler<IndexQuery, IEnumerable<IndexViewModel>>
    {
        private readonly SubAccountService subAccountService;

        public SubAccountHandler(SubAccountService subAccountService)
        {
            this.subAccountService = subAccountService;
        }

        public async Task<IEnumerable<IndexViewModel>> Handle(IndexQuery message)
        {
            var subAccounts = await subAccountService.GetSubAccountsAsync(message.CustId);

            return subAccounts.Select(item => new IndexViewModel(item));
        }
    }
}