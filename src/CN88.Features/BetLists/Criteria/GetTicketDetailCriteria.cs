namespace CN88.Features.BetLists.Criteria
{
    using System;
    using Fanex.Data.Repository;
    using Params;

    public class GetTicketDetailCriteria : CriteriaBase
    {
        public GetTicketDetailCriteria(
            GetTicketDetailParam param,
            DateTime moveBetsDate,
            DateTime balanceUpDay)
        {
            RefNo = param.RefNo;
            TransId = param.TransId;
            BetId = param.BetId;
            CustId = param.CustId;

            MoveBetsDay = moveBetsDate;
            BalanceUpDay = balanceUpDay;
        }

        public string RefNo { get; }

        public string TransId { get; }

        public string BetId { get; }

        public int CustId { get; }

        public DateTime MoveBetsDay { get; }

        public DateTime BalanceUpDay { get; }

        public decimal Outstanding { get; }

        public override string GetSettingKey() => "CS_BetList_Parlay_TicketDetail";

        public override bool IsValid() => CustId > 0;
    }
}
