namespace CN88.Features.BetLists.Params
{
    public class GetParlaySubTicketsParam
    {
        public GetParlaySubTicketsParam(string transId, int custId)
        {
            TransId = transId;
            CustId = custId;
        }

        public string TransId { get; }

        public int CustId { get; }
    }
}