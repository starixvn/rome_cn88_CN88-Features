namespace CN88.Features.CustomerList.Params
{
    using System;

    public class MemberStatementParam
    {
        public int CustId { get; set; }

        public int SportType { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}
