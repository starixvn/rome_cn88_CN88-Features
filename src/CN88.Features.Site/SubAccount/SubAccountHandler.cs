namespace CN88.Features.Site.SubAccount
{
    using System.Threading.Tasks;
    using Alpha.Web.Helpers;
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
        IRequestHandler<NewQuery, NewViewModel>,
        IAsyncRequestHandler<UpdateQuery, SubAccountItem>,
        IAsyncRequestHandler<NewCommand, Result>,
        IAsyncRequestHandler<UpdateCommand, Result>
    {
        private const string HashKeySessionKey = "Alpha.CN88.Site.Handlers.PasswordHandler.PasswordHash";

        private readonly SubAccountService subAccountService;
        private readonly Encryptor encryptor;
        private readonly SessionManager sessionManager;

        public SubAccountHandler(
            SubAccountService subAccountService,
            Encryptor encryptor,
            SessionManager sessionManager)
        {
            this.subAccountService = subAccountService;
            this.encryptor = encryptor;
            this.sessionManager = sessionManager;
        }

        public async Task<IndexViewModel> Handle(IndexQuery message)
            => new IndexViewModel(await subAccountService.GetSubAccountsAsync(message.ViewerId));

        public NewViewModel Handle(NewQuery message)
            => new NewViewModel(message.ViewerName + SubAccountConst.SubAccountPadding, GenerateHashKey());

        public async Task<SubAccountItem> Handle(UpdateQuery message)
            => await subAccountService.GetSubAccountAsync(message.ViewerId, message.CustId);

        public async Task<Result> Handle(NewCommand message)
            => await subAccountService.CreateSubAccountAsync(new CreateSubAccountParam(
                message.Suffix1,
                message.Suffix2,
                message.Password,
                HashKey,
                message.InvalidPasswords,
                message.Name,
                message.PageAccess,
                message.LanguageKey,
                message.IP,
                message.ViewerId,
                message.ViewerName));

        public async Task<Result> Handle(UpdateCommand message)
            => await subAccountService.UpdateSubAccountAsync(new UpdateSubAccountParam(
                message.CustId,
                message.Name,
                message.PageAccess,
                message.LanguageKey,
                message.IP,
                message.ViewerId));

        private string GenerateHashKey()
        {
            var hashKey = encryptor.RandomString(4);
            sessionManager.Add(HashKeySessionKey, hashKey);

            return hashKey;
        }

        private string HashKey
            => sessionManager.Get<string>(HashKeySessionKey);
    }
}