namespace CN88.Features.CustomerList.Models
{
    using System;
    using Extensions;

    public class OnlineMemberModel
    {
        public string UserName { get; }

        public string Name { get; }

        public DateTime LastLogin { get; }

        public string LastLoginStr => LastLogin.ToLongDateFormat();
    }
}
