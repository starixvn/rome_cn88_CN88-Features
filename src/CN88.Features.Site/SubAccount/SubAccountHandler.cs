namespace CN88.Features.Site.SubAccount
{
    using System.Net;
    using System.Threading.Tasks;
    using Alpha.Web.Helpers;
    using CN88.Web.Helpers;
    using Features.Models;
    using Features.SubAccount.Constants;
    using Features.SubAccount.Models;
    using Features.SubAccount.Params;
    using Features.SubAccount.Services;
    using MediatR;
    using Requests;
    using Sunplus.Agent.Utilities;
    using ViewModels;

    public class SubAccountHandler :
        IAsyncRequestHandler<IndexQuery, IndexViewModel>,
        IAsyncRequestHandler<NewQuery, NewViewModel>,
        IAsyncRequestHandler<UpdateQuery, SubAccountItem>,
        IAsyncRequestHandler<NewCommand, Result>,
        IAsyncRequestHandler<UpdateCommand, Result>,
        IAsyncRequestHandler<UnlockCommand, Result>,
        IAsyncRequestHandler<DeleteCommand, Result>
    {
        private const string HashKeySessionKey = "Alpha.CN88.Site.Handlers.PasswordHandler.PasswordHash";

        private readonly SubAccountService subAccountService;
        private readonly Encryptor encryptor;
        private readonly SessionManager sessionManager;
        private readonly BrowserHelper browserHelper;

        public SubAccountHandler(
            SubAccountService subAccountService,
            Encryptor encryptor,
            SessionManager sessionManager,
            BrowserHelper browserHelper)
        {
            this.subAccountService = subAccountService;
            this.encryptor = encryptor;
            this.sessionManager = sessionManager;
            this.browserHelper = browserHelper;
        }

        public async Task<IndexViewModel> Handle(IndexQuery message)
        {
            var showActionButtons = message.ShowActionButtons;
            var showAccountManagementPermission = message.ShowAccountManagementPermission;
            var subAccounts = await subAccountService.GetSubAccountsAsync(message.ViewerId);

            return new IndexViewModel(subAccounts, showActionButtons, showAccountManagementPermission, browserHelper);
        }

        public async Task<NewViewModel> Handle(NewQuery message)
            => new NewViewModel(
                message.ViewerName + SubAccountConst.SubAccountPadding,
                GenerateHashKey(),
                message.ShowAccountManagementPermission,
                await subAccountService.GetManagedAccounts(message.ViewerId, message.ViewerLevelId, message.SubAccountId));

        public async Task<SubAccountItem> Handle(UpdateQuery message)
            => await subAccountService.GetSubAccountAsync(message.ViewerId, message.CustId);

        public async Task<Result> Handle(NewCommand message)
            => await subAccountService.CreateSubAccountAsync(new CreateSubAccountParam(
                message.Suffix1,
                message.Suffix2,
                message.Password,
                HashKey,
                message.InvalidPasswords,
                HtmlEncodeText(message.Name),
                message.PageAccess,
                message.LanguageKey,
                message.IP,
                message.ViewerId,
                message.ViewerName));

        public async Task<Result> Handle(UpdateCommand message)
            => await subAccountService.UpdateSubAccountAsync(new UpdateSubAccountParam(
                message.CustId,
                HtmlEncodeText(message.Name),
                message.PageAccess,
                message.LanguageKey,
                message.IP,
                message.ViewerId));

        public async Task<Result> Handle(UnlockCommand message)
            => await subAccountService.UnlockSubAccountAsync(new UnlockSubAccountParam(
                message.ViewerId,
                message.CustId,
                message.IP,
                message.LanguageKey));

        public async Task<Result> Handle(DeleteCommand message)
            => await subAccountService.DeleteSubAccountAsync(new UnlockSubAccountParam(
                message.ViewerId,
                message.CustId,
                message.IP,
                message.LanguageKey));

        private string GenerateHashKey()
        {
            var hashKey = encryptor.RandomString(4);
            sessionManager.Add(HashKeySessionKey, hashKey);

            return hashKey;
        }

        private string HashKey
            => sessionManager.Get<string>(HashKeySessionKey);

        private static string HtmlEncodeText(string text)
            => string.IsNullOrWhiteSpace(text) ? text : WebUtility.HtmlEncode(text);
    }
}