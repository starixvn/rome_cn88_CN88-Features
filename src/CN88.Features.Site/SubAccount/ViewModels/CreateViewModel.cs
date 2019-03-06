namespace CN88.Features.Site.SubAccount.ViewModels
{
    public class CreateViewModel
    {
        public CreateViewModel(string hashKey)
        {
            HashKey = hashKey;
        }

        public string HashKey { get; }
    }
}