namespace CN88.Features.BetLists.Params
{
    public class GetTicketDetailParam
    {
        public GetTicketDetailParam(
            long refNo,
            long transId,
            long betId,
            int custId)
        {
            RefNo = refNo;
            TransId = transId;
            BetId = betId;
            CustId = custId;
        }

        public long RefNo { get; }

        public long TransId { get; }

        public long BetId { get; }

        public int CustId { get; }
    }
}
