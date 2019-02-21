namespace CN88.Features.SubAccount.Criteria
{
    public class UpdateSubAccountCommand : BaseSubAccountCommand
    {
        public UpdateSubAccountCommand(
            int custId,
            int subAccId,
            int loginSubAccId,
            string firstName,
            string lastName,
            bool closed,
            string languageKey,
            string loginIP,
            int userId) : base(custId, subAccId, loginSubAccId, languageKey, loginIP, userId)
        {
            FirstName = firstName;
            LastName = lastName;
            Closed = closed;
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
