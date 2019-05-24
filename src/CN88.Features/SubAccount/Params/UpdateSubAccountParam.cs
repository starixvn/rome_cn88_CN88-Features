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
            int actorId) : base(name, pageAccess, languageKey, ip, actorId)
        {
            SubAccId = subAccId;
        }

        public int SubAccId { get; }
    }
}