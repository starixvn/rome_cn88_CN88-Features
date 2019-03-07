namespace CN88.Features.Site.Reports.ViewModels
{
    using Features.Models;

    public class VerifyPasswordViewModel
    {
        public VerifyPasswordViewModel(
            string nickName,
            string hashKey,
            Result result)
        {
            NickName = nickName;
            HashKey = hashKey;
            Result = result;
        }

        public string NickName { get; }

        public string HashKey { get; }

        public Result Result { get; }
    }
}