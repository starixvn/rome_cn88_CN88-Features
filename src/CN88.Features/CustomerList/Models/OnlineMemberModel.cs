namespace CN88.Features.CustomerList.Models
{
    using System;
    using Extensions;

    public class OnlineMemberModel
    {
        public OnlineMemberModel()
        {
        }

        public OnlineMemberModel(string userCode, string name, DateTime loginTime)
        {
            UserCode = userCode;
            Name = name;
            LoginTime = loginTime;
        }

        public string UserCode { get; }

        public string Name { get; }

        public DateTime LoginTime { get; }

        public string LoginTimeStr => LoginTime.ToShortDateFormat();
    }
}
