namespace CN88.Features.CustomerList.Criteria
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetMemberStatementCriteria : CriteriaBase
    {
        public GetMemberStatementCriteria(
            int custId, 
            int sportType, 
            DateTime fromDate, 
            DateTime toDate,
            DateTime moveBetsDay,
            DateTime balanceUpDay)
        {
            CustId = custId;
            SportType = sportType;
            FromDate = fromDate;
            ToDate = toDate;
            MoveBetsDay = moveBetsDay;
            BalanceUpDay = balanceUpDay;
        }

        public int CustId { get; }

        [SpParam(SqlParamName = "SportTypeGroup")]
        public int SportType { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime MoveBetsDay { get; }

        public DateTime BalanceUpDay { get; }

        public override string GetSettingKey()
            => "CS_Rpt_MemberStatement_Sel";

        public override bool IsValid()
            => CustId > 0 && 
            FromDate > DateTime.MinValue && 
            ToDate > DateTime.MinValue && 
            MoveBetsDay > DateTime.MinValue && 
            BalanceUpDay > DateTime.MinValue;
    }
}
