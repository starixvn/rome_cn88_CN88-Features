namespace CN88.Features.CustomerList.Params
{
    public class UnlockCustomerParam
    {
        public UnlockCustomerParam(
           int searchAccountId,
           int actorId,
           int subAccountId,
           string language,
           string ip)
        {
            SearchAccountId = searchAccountId;
            ActorId = actorId;
            SubAccountId = subAccountId;
            Language = language;
            IP = ip;
        }

        public int SearchAccountId { get; }

        public byte SearchAccountRoleId { get; }

        public int ActorId { get; }

        public int SubAccountId { get; }

        public string Language { get; }

        public string IP { get; }
    }
}
