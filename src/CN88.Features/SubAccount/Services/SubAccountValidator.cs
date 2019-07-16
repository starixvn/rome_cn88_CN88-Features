namespace CN88.Features.SubAccount.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using AppResources;
    using CN88.Features.Shared.Extensions;
    using Fanex.Cryptography.Aes;
    using Features.Models;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;

    public interface SubAccountValidator
    {
        string Encode(string password);

        string Decrypt(string password, string encryptKey);

        Result ValidateEmptyPassword(string password);

        Result IsMatchPasswordRules(string password, IEnumerable<string> invalidPasswords);

        string FilterWithValidPermissions(string pageAccess);

        Result ValidateSuffix(string suffix1, string suffix2);
    }

    public class SubAccountValidatorImpl : SubAccountValidator
    {
        private static readonly Regex PasswordRule1 = new Regex(@"^(?=.*\d).{6,12}$");
        private static readonly Regex PasswordRule2 = new Regex(@"^(?=.*[a-z]).{6,12}$");
        private static readonly Regex PasswordRule3 = new Regex(@"^(?=.*[A-Z]).{6,12}$");
        private static readonly Regex PasswordRule4 = new Regex(@"^(?=.*[\[\]\^\$\.\|\?\*\+\(\)\\~`\!@#%&\-_+={}'""<>:;,]).{6,12}$");
        private static readonly Regex NoWhiteSpace = new Regex(@"^\S*$");

        /* [B]: Account Management Edit & View
         * [D]: Total Bets
         * [E]: Reports
         * [H]: Account Management View Only
         */

        private readonly Encryptor encryptor;

        public SubAccountValidatorImpl(Encryptor encryptor)
        {
            this.encryptor = encryptor;
        }

        public string Encode(string password)
            => encryptor.CfsEnCode(password);

        public string Decrypt(string password, string encryptKey)
            => AesExtension.Decrypt(password, encryptKey);

        public Result ValidateEmptyPassword(string password)
            => string.IsNullOrWhiteSpace(password)
            ? Result.CreateErrorResult(message: SubAccountResource.RequiredPasswordError)
            : Result.CreateSuccessResult();

        public Result IsMatchPasswordRules(string password, IEnumerable<string> invalidPasswords)
        {
            if (IsEqualsPasswordHint(password))
            {
                return Result.CreateErrorResult(message: SubAccountResource.PasswordWrongRuleError);
            }

            if (IsContainsInvalidChars(password, invalidPasswords))
            {
                return Result.CreateErrorResult(message: SubAccountResource.NewPwdContainsUserNameOrNickNameError);
            }

            var rule1 = PasswordRule1.Match(password).Success ? 1 : 0;
            var rule2 = PasswordRule2.Match(password).Success ? 1 : 0;
            var rule3 = PasswordRule3.Match(password).Success ? 1 : 0;
            var rule4 = PasswordRule4.Match(password).Success ? 1 : 0;

            return (rule1 + rule2 + rule3 + rule4 >= 2) && NoWhiteSpace.Match(password).Success
                ? Result.CreateSuccessResult()
                : Result.CreateErrorResult(message: SubAccountResource.PasswordWrongRuleError);
        }

        public string FilterWithValidPermissions(string pageAccess)
            => string.IsNullOrWhiteSpace(pageAccess)
            ? string.Empty
            : pageAccess
                .SplitString()
                .Intersect(new[] { PageAccessType.ViewTotalBets, PageAccessType.ViewReports })
                .JoinWithComma();

        private static bool IsEqualsPasswordHint(string password)
            => password.Equals("BCD!$#H", StringComparison.OrdinalIgnoreCase) ||
            password.Equals("4593abcd", StringComparison.OrdinalIgnoreCase);

        private static bool IsContainsInvalidChars(string password, IEnumerable<string> invalidPasswords)
            => invalidPasswords.Any(item => password.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0);

        public Result ValidateSuffix(string suffix1, string suffix2)
            => ValidateSuffix(suffix1) && ValidateSuffix(suffix2)
            ? Result.CreateSuccessResult()
            : Result.CreateForbiddenResult();

        private static bool ValidateSuffix(string suffix)
        {
            int value;

            return suffix?.Length == 1 && int.TryParse(suffix, out value);
        }
    }
}