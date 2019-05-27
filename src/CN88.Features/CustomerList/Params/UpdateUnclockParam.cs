namespace CN88.Features.CustomerList.Params
{
    public class UpdateUnclockParam
    {
        public UpdateUnclockParam(
           int searchAccountId,
           bool locked,
           int actorId,
           int subAccountId,
           string language,
           string ip,
           bool isInternal)
        {
            SearchAccountId = searchAccountId;
            ActorId = actorId;
            SubAccountId = subAccountId;
            Language = language;
            IP = ip;
            Locked = locked;
        }

        public int SearchAccountId { get; }

        public byte SearchAccountRoleId { get; }

        public int ActorId { get; }

        public int SubAccountId { get; }

        public string Language { get; }

        public string IP { get; }

        public bool Locked { get; }
    }
}
