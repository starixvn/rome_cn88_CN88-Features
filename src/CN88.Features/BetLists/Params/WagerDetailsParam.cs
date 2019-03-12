namespace CN88.Features.BetLists.Params
{
    using System;

    public class WagerDetailsParam
    {
        public WagerDetailsParam(int custId, DateTime limitedDate)
        {
            CustId = custId;
            LimitedDate = limitedDate;
        }

        public int CustId { get; }

        public DateTime LimitedDate { get; }
    }
}
