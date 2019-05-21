namespace CN88.Features.SubAccount.Params
{
    public class UnlockSubAccountParam
    {
        public UnlockSubAccountParam(
            int custId,
            int subAccId,
            string ip,
            string languageKey)
        {
            CustId = custId;
            SubAccId = subAccId;
            IP = ip;
            LanguageKey = languageKey;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public string IP { get; }

        public string LanguageKey { get; }
    }
}