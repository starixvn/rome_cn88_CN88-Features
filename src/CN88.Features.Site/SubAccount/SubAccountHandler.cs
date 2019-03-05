namespace CN88.Features.Site.SubAccount
{
    using System.Linq;
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

            var subAccountsTemp = subAccounts.ToList();
            for (int i = 0; i < 100; i++)
            {
                subAccountsTemp.Add(subAccountsTemp[i]);
            }

            return new IndexViewModel(
                message.UserCode,
                message.PageIndex,
                message.PageSize,
                15,
                subAccountsTemp);

            //return new IndexViewModel(
            //    message.UserCode,
            //    message.PageIndex,
            //    message.PageSize,
            //    15,
            //    subAccounts);
        }
    }
}