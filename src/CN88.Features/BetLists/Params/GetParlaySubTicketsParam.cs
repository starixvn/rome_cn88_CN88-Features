namespace CN88.Features.BetLists.Params
{
    public class GetParlaySubTicketsParam
    {
        public GetParlaySubTicketsParam(string transId, int custId, int headlineId)
        {
            TransId = transId;
            CustId = custId;
            HeadlineId = headlineId;
        }

        public string TransId { get; }

        public int CustId { get; }

        public int HeadlineId { get; }
    }
}