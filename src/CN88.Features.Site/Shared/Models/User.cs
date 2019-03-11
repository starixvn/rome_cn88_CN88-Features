namespace CN88.Features.Site.Models
{
    using System;
    using Features.Models;
    using Sunplus.Agent.Model.Entities;

    public class User : IUser
    {
        private readonly IUserSession userSession;

        private readonly PageAccess pageAccess;

        public User(IUserSession userSession)
        {
            this.userSession = userSession;
            pageAccess = new PageAccess();
        }

        public int Id => userSession.CustId;

        public string Name => userSession.UserName;

        public string NickName => userSession.NickName;

        public string Language => userSession.Language;

        public string LanguageKey => userSession.Language;

        public byte LevelId => userSession.RoleId;

        public bool IsInternal => userSession.IsInternal;

        public int SubAccountId => userSession.SubAccId;

        public string IP => userSession.UserLoginIp;

        public bool HasUpdatePermission => !userSession.IsSubAcc || pageAccess.UpdateAccount;

        public bool HasAddNewPermission => !userSession.IsSubAcc || pageAccess.CreateAccount;

        public bool IsAdminCanViewVNIP => userSession.IsAdminCanViewVNIP;

        public int AdminId => userSession.AdminId;

        public DateTime LimitedDateToShowInfo => userSession.GetProperty<DateTime>("CustomerDateLimit");
    }
}