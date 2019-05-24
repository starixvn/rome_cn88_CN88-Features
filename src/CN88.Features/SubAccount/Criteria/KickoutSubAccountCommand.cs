namespace CN88.Features.SubAccount.Criteria
{
    using Fanex.Data.Repository;

    public class KickoutSubAccountCommand : NonQueryCommand
    {
        public KickoutSubAccountCommand(int custId, int subAccId)
        {
            CustId = custId;
            SubAccId = subAccId;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public override string GetSettingKey()
            => "Age_On_UserKickedout";

        public override bool IsValid()
            => CustId > 0 && SubAccId > 0;
    }
}