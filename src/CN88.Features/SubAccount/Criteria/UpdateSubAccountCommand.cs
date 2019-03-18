namespace CN88.Features.SubAccount.Criteria
{
    using Params;

    public class UpdateSubAccountCommand : BaseSubAccountCommand
    {
        public UpdateSubAccountCommand(UpdateSubAccountParam param)
            : base(param.CustId, param.SubAccId, param.LoginSubAccId, param.LanguageKey, param.IP, param.ActorId)
        {
            FirstName = param.FirstName;
            LastName = param.LastName;
            Closed = param.Closed;
        }

        public string FirstName { get; }

        public string LastName { get; }

        public bool Closed { get; }

        public override string GetSettingKey()
               => "Age_CustSubUpdInfo";

        public override bool IsValid()
            => base.IsValid() && SubAccId > 0;
    }
}
