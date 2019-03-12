namespace CN88.Features.BetLists.Params
{
    using System;

    public class BetListByAccountParam
    {
        public BetListByAccountParam(int custId, DateTime limitedDate)
        {
            CustId = custId;
            LimitedDate = limitedDate;
        }

        public int CustId { get; }

        public DateTime LimitedDate { get; }
    }
}
