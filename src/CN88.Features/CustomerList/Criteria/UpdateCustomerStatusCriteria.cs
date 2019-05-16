namespace CN88.Features.CustomerList.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class UpdateCustomerStatusCriteria : NonQueryCommand
    {
        public UpdateCustomerStatusCriteria(
            int searchAccountId,
            int actorId,
            int subAccountId,
            byte updateStatusId,
            string language,
            string ip)
        {
            SearchAccountId = searchAccountId;
            ActorId = actorId;
            SubAccountId = subAccountId;
            UpdateStatusId = updateStatusId;
            Language = language;
            IP = ip;
        }

        [SpParam(SqlParamName = "CustId")]
        public int SearchAccountId { get; }

        [SpParam(SqlParamName = "UserId")]
        public int ActorId { get; }

        [SpParam(SqlParamName = "SubAccId")]
        public int SubAccountId { get; }

        [SpParam(SqlParamName = "UpdStatusId")]
        public byte UpdateStatusId { get; }

        public byte SystemID => 2;

        public string Language { get; }

        public string IP { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "CS_Customer_Status_Upd";

        public override bool IsValid() => SearchAccountId > 0 && ActorId > 0;
    }
}