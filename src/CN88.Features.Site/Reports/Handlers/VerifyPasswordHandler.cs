namespace CN88.Features.Site.Reports.Handlers
{
    using Alpha.Web.Helpers;
    using Fanex.Cryptography.Aes;
    using Features.Models;
    using Features.Reports.Services;
    using MediatR;
    using Requests;
    using Sunplus.Agent.Utilities;
    using ViewModels;

    public class VerifyPasswordHandler :
        IRequestHandler<VerifyPasswordQuery, VerifyPasswordViewModel>,
        IRequestHandler<VerifyPasswordCommand, Result>
    {
        private const string HashKeySessionKey = "CN88.Features.Site.Reports.Handlers.VerifyPasswordHandler.PasswordHash";

        private readonly Encryptor encryptor;
        private readonly SessionManager sessionManager;
        private readonly PasswordService passwordService;

        public VerifyPasswordHandler(
            Encryptor encryptor,
            SessionManager sessionManager,
            PasswordService passwordService)
        {
            this.encryptor = encryptor;
            this.sessionManager = sessionManager;
            this.passwordService = passwordService;
        }

        public VerifyPasswordViewModel Handle(VerifyPasswordQuery message)
            => new VerifyPasswordViewModel(message.NickName, GetHashKey(), message.Result);

        public Result Handle(VerifyPasswordCommand message)
        {
            var rawPassword = AesExtension.Decrypt(message.Password, sessionManager.Get<string>(HashKeySessionKey));
            var result = passwordService.CheckCustomerPassword(
            message.ViewerId,
            message.SubAccountId,
            encryptor.CfsEnCode(rawPassword));

            return result;
        }

        private string GetHashKey()
        {
            var hashKey = encryptor.RandomString(4);
            sessionManager.Add(HashKeySessionKey, hashKey);

            return hashKey;
        }
    }
}