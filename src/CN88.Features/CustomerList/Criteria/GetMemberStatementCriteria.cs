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
            //CustId = custId;
            //SportType = sportType;
            //FromDate = fromDate;
            //ToDate = toDate;
            //MoveBetsDay = moveBetsDay;
            //BalanceUpDay = balanceUpDay;
        }

        public int CustId=> 34109860;

        [SpParam(SqlParamName = "SportTypeGroup")]
        public int SportType => 0;

        public DateTime FromDate => new DateTime(2019, 2, 20);

        public DateTime ToDate => new DateTime(2019, 2, 27);

        public DateTime MoveBetsDay => new DateTime(2019, 2, 16);

        public DateTime BalanceUpDay => new DateTime(2019, 2, 8);

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
