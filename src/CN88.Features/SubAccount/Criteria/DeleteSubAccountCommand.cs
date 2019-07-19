namespace CN88.Features.SubAccount.Criteria
{
    using Fanex.Data.Repository;

    public class DeleteSubAccountCommand : NonQueryCommand
    {
        public DeleteSubAccountCommand(
            int custId,
            int subAccId,
            string ip,
            string language)
        {
            CustId = custId;
            SubAccId = subAccId;
            LoginIP = ip;
            Language = language;
            UserId = custId;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public string LoginIP { get; }

        public string Language { get; }

        public int UserId { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "CS_CustSubDel";

        public override bool IsValid()
            => CustId > 0 && SubAccId > 0 && UserId > 0;
    }
}
