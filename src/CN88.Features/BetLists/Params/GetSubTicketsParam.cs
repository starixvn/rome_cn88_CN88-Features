namespace CN88.Features.BetLists.Params
{
    public class GetSubTicketsParam
    {
        public GetSubTicketsParam(long transId, int custId)
        {
            TransId = transId;
            CustId = custId;
        }

        public long TransId { get; }

        public int CustId { get; }
    }
}
