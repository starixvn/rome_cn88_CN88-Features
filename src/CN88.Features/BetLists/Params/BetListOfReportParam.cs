namespace CN88.Features.BetLists.Params
{
    using System;

    public class BetListOfReportParam
    {
        public BetListOfReportParam(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            int sportTypeGroup,
            int betTypeGroup,
            int liveIndicator,
            int type)
        {
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
            SportTypeGroup = sportTypeGroup;
            BetTypeGroup = betTypeGroup;
            LiveIndicator = liveIndicator;
            Type = type;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int SportTypeGroup { get; }

        public int BetTypeGroup { get; }

        public int LiveIndicator { get; }

        public int Type { get; }
    }
}
