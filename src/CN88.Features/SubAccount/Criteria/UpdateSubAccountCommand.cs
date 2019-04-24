namespace CN88.Features.SubAccount.Criteria
{
    using Params;

    public class UpdateSubAccountCommand : BaseSubAccountCommand
    {
        public UpdateSubAccountCommand(UpdateSubAccountParam param)
            : base(param.ActorId, param.Name, param.PageAccess, param.LanguageKey, param.IP, param.ActorId)
        {
            SubAccId = param.SubAccId;
        }

        public int SubAccId { get; }

        public override string GetSettingKey()
            => "CS_CustSubUpd";

        public override bool IsValid()
            => base.IsValid() && SubAccId > 0;
    }
}
