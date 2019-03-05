namespace CN88.Features.Site.Reports.ViewModels
{
    public class VerifyPasswordViewModel
    {
        public VerifyPasswordViewModel(
            string nickName,
            string hashKey)
        {
            NickName = nickName;
            HashKey = hashKey;
        }

        public string NickName { get; }

        public string HashKey { get; }
    }
}