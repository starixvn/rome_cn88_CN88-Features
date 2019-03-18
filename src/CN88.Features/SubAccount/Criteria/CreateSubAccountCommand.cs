namespace CN88.Features.SubAccount.Criteria
{
    using Params;

    public class CreateSubAccountCommand : BaseSubAccountCommand
    {
        public CreateSubAccountCommand(CreateSubAccountParam param)
            : base(param.CustId, 0, param.LoginSubAccId, param.LanguageKey, param.IP, param.ActorId)
        {
            UserName = param.UserName;
            UserPwd = param.Password;
            FirstName = param.FirstName;
            LastName = param.LastName;
            PageAccess = param.PageAccess;
        }

        public string UserName { get; }

        public string UserPwd { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string PageAccess { get; }

        public bool Success { get; set; }

        public override string GetSettingKey()
               => "Age_CustSubIns";

        public override bool IsValid()
            => base.IsValid()
            && !string.IsNullOrWhiteSpace(UserName)
            && !string.IsNullOrWhiteSpace(UserPwd);
    }
}
