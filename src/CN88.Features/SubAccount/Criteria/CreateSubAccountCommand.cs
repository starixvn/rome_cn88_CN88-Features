namespace CN88.Features.SubAccount.Criteria
{
    public class CreateSubAccountCommand : BaseSubAccountCommand
    {
        public CreateSubAccountCommand(
            int custId,
            int loginSubAccId,
            string userName,
            string userPwd,
            string firstName,
            string lastName,
            string pageAccess,
            string languageKey,
            string loginIP,
            int userId) : base(custId, 0, loginSubAccId, languageKey, loginIP, userId)
        {
            UserName = userName;
            UserPwd = userPwd;
            FirstName = firstName;
            LastName = lastName;
            PageAccess = pageAccess;
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
