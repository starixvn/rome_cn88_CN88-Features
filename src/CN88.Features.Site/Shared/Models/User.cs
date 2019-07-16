namespace CN88.Features.Site.Models
{
    using System;
    using Features.Models;
    using Sunplus.Agent.Model.Entities;

    public class User : IUser
    {
        private readonly IUserSession userSession;

        public User(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public int Id => userSession.CustId;

        public string Name => userSession.UserName;

        public string NickName => userSession.NickName;

        public string Language => userSession.Language;

        public string LanguageKey => userSession.LangKey;

        public byte LevelId => userSession.RoleId;

        public bool IsInternal => userSession.IsInternal;

        public int SubAccountId => userSession.SubAccId;

        public bool IsSubAccount => userSession.IsSubAcc;

        public string IP => userSession.UserLoginIp;

        public bool AccountManagementEditView => userSession.SubPageAccess.UpdateAccount;

        public bool AccountManagementViewOnly => userSession.SubPageAccess.ViewMemberInfo;

        public bool IsAdminCanViewVNIP => userSession.IsAdminCanViewVNIP;

        public int AdminId => userSession.AdminId;

        public DateTime LimitedDateToShowInfo => userSession.GetProperty<DateTime>("CustomerDateLimit");

        public byte StatusId => userSession.GetProperty<byte>("CUSTOMERSTATUS");

        public bool IsAllDownline => userSession.GetProperty<bool>("ISALLDOWNLINE");
    }
}