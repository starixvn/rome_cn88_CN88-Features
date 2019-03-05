namespace CN88.Features.Site.SubAccount
{
    using System.Threading.Tasks;
    using Features.SubAccount.Services;
    using MediatR;
    using Requests;
    using ViewModels;

    public class SubAccountHandler :
        IAsyncRequestHandler<IndexQuery, IndexViewModel>
    {
        private readonly SubAccountService subAccountService;

        public SubAccountHandler(SubAccountService subAccountService)
        {
            this.subAccountService = subAccountService;
        }

        public async Task<IndexViewModel> Handle(IndexQuery message)
        {
            var subAccounts = await subAccountService.GetSubAccountsAsync(message.ViewerId);

            return new IndexViewModel(
                message.UserCode,
                message.PageIndex,
                message.PageSize,
                15,
                subAccounts);
        }
    }
}