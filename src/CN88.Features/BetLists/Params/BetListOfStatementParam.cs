namespace CN88.Features.BetLists.Params
{
    using System;

    public class BetListOfStatementParam
    {
        public BetListOfStatementParam(int custId, DateTime date, int sportTypeGroup)
        {
            CustId = custId;
            Date = date;
            SportTypeGroup = sportTypeGroup;
        }

        public int CustId { get; }

        public DateTime Date { get; }

        public int SportTypeGroup { get; }
    }
}