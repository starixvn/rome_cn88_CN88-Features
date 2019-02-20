namespace CN88.Features.Site.Models
{
    using CN88.Features.Models;
    using Sunplus.Agent.Model.Entities;

    public class User : IUser
    {
#pragma warning disable CS0618 // Type or member is obsolete
        private readonly IUserSession userSession;
#pragma warning restore CS0618 // Type or member is obsolete

        private readonly PageAccess pageAccess;

#pragma warning disable CS0618 // Type or member is obsolete
        public User(IUserSession userSession)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            this.userSession = userSession;
            pageAccess = new PageAccess();
        }

        public int Id => userSession.CustId;

        public string Name => userSession.UserName;

        public string Language => userSession.Language;

        public byte LevelId => 4; // userSession.RoleId;

        public bool IsInternal => userSession.IsInternal;

        public int SubAccountId => userSession.SubAccId;

        public string IP => userSession.UserLoginIp;

        public bool HasUpdatePermission => !userSession.IsSubAcc || pageAccess.UpdateAccount;

        public bool HasAddNewPermission => !userSession.IsSubAcc || pageAccess.CreateAccount;
    }
}