namespace CN88.Features.SubAccount.Criteria
{
    using Params;

    public class CreateSubAccountCommand : BaseSubAccountCommand
    {
        public CreateSubAccountCommand(CreateSubAccountParam param)
            : base(param.ActorId, param.Name, param.PageAccess, param.LanguageKey, param.IP, param.ActorId)
        {
            UserName = param.UserName;
            UserPwd = param.Password;
        }

        public string UserName { get; }

        public string UserPwd { get; }

        public int SubAccId { get; set; }

        public override string GetSettingKey()
            => "CS_CustSubIns";

        public override bool IsValid()
            => base.IsValid()
            && !string.IsNullOrWhiteSpace(UserName)
            && !string.IsNullOrWhiteSpace(UserPwd);
    }
}