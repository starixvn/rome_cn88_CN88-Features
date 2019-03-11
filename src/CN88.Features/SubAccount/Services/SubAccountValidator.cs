namespace CN88.Features.SubAccount.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using AppResources;
    using Fanex.Cryptography.Aes;
    using Features.Models;
    using Sunplus.Agent.Utilities;

    public interface SubAccountValidator
    {
        string Encode(string password);

        string Decrypt(string password, string encryptKey);

        Result ValidateEmpty(string password);

        Result IsMatchPasswordRules(string password, IEnumerable<string> invalidPasswords);

        string FilterWithValidPermissions(string pageAccess);
    }

    public class SubAccountValidatorImpl : SubAccountValidator
    {
        private const int InvalidPasswordRulesErrorCode = 1001;

        private static readonly Regex PasswordRule1 = new Regex(@"^(?=.*\d).{8,100}$");
        private static readonly Regex PasswordRule2 = new Regex(@"^(?=.*[a-z]).{8,100}$");
        private static readonly Regex PasswordRule3 = new Regex(@"^(?=.*[A-Z]).{8,100}$");
        private static readonly Regex PasswordRule4 = new Regex(@"^(?=.*[\[\]\^\$\.\|\?\*\+\(\)\\~`\!@#%&\-_+={}'""<>:;,]).{8,100}$");
        private static readonly Regex MemberPasswordRule = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,10}$");
        private static readonly Regex NoWhiteSpace = new Regex(@"^\S*$");

        private static readonly string[] ValidPermissions = new[] { "[A]", "[B]", "[D]", "[E]", "[H]" };

        private readonly Encryptor encryptor;

        public SubAccountValidatorImpl(Encryptor encryptor)
        {
            this.encryptor = encryptor;
        }

        public string Encode(string password)
            => encryptor.CfsEnCode(password);

        public string Decrypt(string password, string encryptKey)
            => AesExtension.Decrypt(password, encryptKey);

        public Result ValidateEmpty(string newPassword)
            => string.IsNullOrWhiteSpace(newPassword)
            ? Result.CreateErrorResult(InvalidPasswordRulesErrorCode, SubAccountResource.RequiredPasswordError)
            : Result.CreateSuccessResult();

        public Result IsMatchPasswordRules(string password, IEnumerable<string> invalidPasswords)
        {
            if (IsEqualsPasswordHint(password))
            {
                return Result.CreateErrorResult(InvalidPasswordRulesErrorCode, SubAccountResource.PasswordWrongRuleError);
            }

            if (IsContainsInvalidChars(password, invalidPasswords))
            {
                return Result.CreateErrorResult(InvalidPasswordRulesErrorCode, SubAccountResource.NewPwdContainsUserNameOrNickNameError);
            }

            var rule1 = PasswordRule1.Match(password).Success ? 1 : 0;
            var rule2 = PasswordRule2.Match(password).Success ? 1 : 0;
            var rule3 = PasswordRule3.Match(password).Success ? 1 : 0;
            var rule4 = PasswordRule4.Match(password).Success ? 1 : 0;

            return (rule1 + rule2 + rule3 + rule4 >= 3) && NoWhiteSpace.Match(password).Success
                ? Result.CreateSuccessResult()
                : Result.CreateErrorResult(InvalidPasswordRulesErrorCode, SubAccountResource.PasswordWrongRuleError);
        }

        public string FilterWithValidPermissions(string pageAccess)
            => string.IsNullOrWhiteSpace(pageAccess)
            ? string.Empty
            : string.Join("", pageAccess.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Intersect(ValidPermissions));

        private static bool IsEqualsPasswordHint(string password)
            => password.Equals("59D7!4$h", StringComparison.OrdinalIgnoreCase) ||
            password.Equals("493abcDE", StringComparison.OrdinalIgnoreCase);

        private static bool IsContainsInvalidChars(string password, IEnumerable<string> invalidPasswords)
            => invalidPasswords.Any(item => password.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0);
    }
}
