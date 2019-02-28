namespace CN88.Features.CustomerList.Criteria
{
    using CN88.Features.Shared.Models;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System.Collections.Generic;

    public class UpdateCreditCriteria : NonQueryCommand
    {
        public UpdateCreditCriteria(
            int searchAccountId,
            decimal creditAmount,
            int actorId,
            int subAccountId,
            string language,
            string ip)
        {
            SearchAccountId = searchAccountId;
            CreditAmount = creditAmount;
            ActorId = actorId;
            SubAccountId = subAccountId;
            Language = language;
            IP = ip;
        }

        [SpParam(SqlParamName = "CustId")]
        public int SearchAccountId { get; }

        [SpParam(Ignored = true)]
        public byte SearchAccountRoleId { get; }

        [SpParam(SqlParamName = "UpdCredit")]
        public decimal CreditAmount { get; }

        [SpParam(SqlParamName = "UserId")]
        public int ActorId { get; }

        [SpParam(SqlParamName = "SubAccId")]
        public int SubAccountId { get; }

        public string Language { get; }

        public string IP { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "CS_Customer_Credit_Upd";

        public override bool IsValid() => SearchAccountId > 0 && ActorId > 0;

    }
}
