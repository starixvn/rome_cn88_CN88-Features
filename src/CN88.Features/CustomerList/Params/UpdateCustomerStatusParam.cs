namespace CN88.Features.CustomerList.Params
{
    public class UpdateCustomerStatusParam
    {
        public UpdateCustomerStatusParam(
            int searchAccountId,
            int actorId,
            int subAccountId,
            byte updateStatusId,
            string language,
            string ip,
            string actorName)
        {
            SearchAccountId = searchAccountId;
            ActorId = actorId;
            SubAccountId = subAccountId;
            UpdateStatusId = updateStatusId;
            Language = language;
            IP = ip;
            ActorName = actorName;
        }

        public int SearchAccountId { get; }

        public int ActorId { get; }

        public int SubAccountId { get; }

        public byte UpdateStatusId { get; }

        public string Language { get; }

        public string IP { get; }

        public string ActorName { get; }
    }
}
