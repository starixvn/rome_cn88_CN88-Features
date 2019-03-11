namespace CN88.Features.Site.SubAccount
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.Web.Helpers;
    using Features.Models;
    using Features.SubAccount.Models;
    using Features.SubAccount.Params;
    using Features.SubAccount.Services;
    using MediatR;
    using Requests;
    using Sunplus.Agent.Utilities;
    using ViewModels;

    public class SubAccountHandler :
        IAsyncRequestHandler<IndexQuery, IndexViewModel>,
        IRequestHandler<CreateQuery, CreateViewModel>,
        IAsyncRequestHandler<UpdateQuery, SubAccountItem>,
        IAsyncRequestHandler<CreateCommand, Result>,
        IAsyncRequestHandler<UpdateCommand, Result>,
        IAsyncRequestHandler<UpdatePermissionCommand, Result>
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
        {
            var subAccounts = await subAccountService.GetSubAccountsAsync(message.ViewerId);

            //var subaccountstemp = subaccounts.tolist();
            //for (int i = 0; i < 100; i++)
            //{
            //    subaccountstemp.add(subaccountstemp[i]);
            //}

            //return new indexviewmodel(
            //    message.usercode,
            //    message.pageindex,
            //    message.pagesize,
            //    15,
            //    subaccountstemp);

            return new IndexViewModel(
                message.UserCode,
                message.PageIndex,
                message.PageSize,
                15,
                subAccounts);
        }

        public CreateViewModel Handle(CreateQuery message)
            => new CreateViewModel(GenerateHashKey());

        public async Task<SubAccountItem> Handle(UpdateQuery message)
            => await subAccountService.GetSubAccountAsync(message.ViewerId, message.CustId);

        public async Task<Result> Handle(CreateCommand message)
            => await subAccountService.CreateSubAccountAsync(new CreateSubAccountParam(
                message.ViewerId,
                message.SubAccountId,
                message.UserName,
                message.Password,
                HashKey,
                message.InvalidPasswords,
                message.FirstName,
                message.LastName,
                message.PageAccess,
                message.LanguageKey,
                message.IP,
                message.ViewerId));

        public async Task<Result> Handle(UpdateCommand message)
            => await subAccountService.UpdateSubAccountAsync(new UpdateSubAccountParam(
                message.ViewerId,
                message.CustId,
                message.SubAccountId,
                message.FirstName,
                message.LastName,
                message.Closed,
                message.LanguageKey,
                message.IP,
                message.ViewerId));

        public async Task<Result> Handle(UpdatePermissionCommand message)
            => await subAccountService.UpdateSubAccountPermissionAsync(new UpdateSubAccountPermissionParam(
                message.ViewerId,
                message.SubAccId,
                message.SubAccountId,
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
        {
            get
            {
                return sessionManager.Get<string>(HashKeySessionKey);
            }
        }
    }
}