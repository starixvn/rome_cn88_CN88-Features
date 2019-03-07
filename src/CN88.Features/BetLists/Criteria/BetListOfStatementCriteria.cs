namespace CN88.Features.BetLists.Criteria
{
    using System;
    using Fanex.Data.Repository;
    using Params;

    public class BetListOfStatementCriteria : CriteriaBase
    {
        public BetListOfStatementCriteria(BetListOfStatementParam param, DateTime moveBetsDay)
        {
            CustId = param.CustId;
            Date = param.Date;
            SportTypeGroup = param.SportTypeGroup;
            MoveBetsDay = moveBetsDay;
        }

        public int CustId { get; }

        public DateTime Date { get; }

        public DateTime MoveBetsDay { get; }

        public int SportTypeGroup { get; }

        public override string GetSettingKey() => "CS_BetList_Statement_Mem";

        public override bool IsValid() => CustId > 0;
    }
}
