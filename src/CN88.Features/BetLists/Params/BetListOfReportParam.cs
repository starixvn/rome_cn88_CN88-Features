namespace CN88.Features.BetLists.Params
{
    using System;

    public class BetListOfReportParam
    {
        public BetListOfReportParam(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            string sportTypeGroupIds,
            int betTypeGroup,
            int liveIndicator,
            int type,
            DateTime limitedDate,
            int headlineId)
        {
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
            SportTypeGroupIds = sportTypeGroupIds;
            BetTypeGroup = betTypeGroup;
            LiveIndicator = liveIndicator;
            Type = type;
            LimitedDate = limitedDate;
            HeadlineId = headlineId;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string SportTypeGroupIds { get; }

        public int BetTypeGroup { get; }

        public int LiveIndicator { get; }

        public int Type { get; }

        public DateTime LimitedDate { get; }

        public int HeadlineId { get; }

        public bool IsHeadline => HeadlineId > 0;
    }
}