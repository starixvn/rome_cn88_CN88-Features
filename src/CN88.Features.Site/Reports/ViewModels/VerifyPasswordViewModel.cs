namespace CN88.Features.Site.Reports.ViewModels
{
    public class VerifyPasswordViewModel
    {
        public VerifyPasswordViewModel(string nickName)
        {
            NickName = nickName;
        }

        public string NickName { get; }
    }
}