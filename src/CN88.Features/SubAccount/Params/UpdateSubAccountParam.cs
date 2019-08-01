namespace CN88.Features.SubAccount.Params
{
    public class UpdateSubAccountParam : BaseSubAccountParam
    {
        public UpdateSubAccountParam(
            int subAccId,
            string name,
            string pageAccess,
            string languageKey,
            string ip,
            int actorId,
            byte roleId,
            bool isAllDownline,
            string downlineId)
            : base(name, pageAccess, languageKey, ip, actorId, roleId, isAllDownline, downlineId)
        {
            SubAccId = subAccId;
        }

        public int SubAccId { get; }
    }
}