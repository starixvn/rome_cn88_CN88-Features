namespace CN88.Features.BetLists.Params
{
    public class GetTicketDetailParam
    {
        public GetTicketDetailParam(
            string refNo,
            string transId,
            string betId,
            int custId)
        {
            RefNo = refNo;
            TransId = transId;
            BetId = betId;
            CustId = custId;
        }

        public string RefNo { get; }

        public string TransId { get; }

        public string BetId { get; }

        public int CustId { get; }
    }
}
