namespace CN88.Features.CustomerList.Params
{
    public class MemberStatementParam
    {
        public MemberStatementParam(
            int custId,
            int sportType,
            int fromDate,
            int toDate)
        {
            CustId = custId;
            SportType = sportType;
            FromDate = fromDate;
            ToDate = toDate;
        }

        public int CustId { get; }

        public int SportType { get; }

        public int FromDate { get; }

        public int ToDate { get; }
    }
}