namespace CN88.Features.BetLists.Criteria
{
    using System;
    using Fanex.Data.Repository;
    using Params;

    public class GetParlayDetailCriteria : CriteriaBase
    {
        public GetParlayDetailCriteria(
            GetParlayDetailParam param,
            DateTime moveBetsDate,
            DateTime balanceUpDay)
        {
            RefNo = param.RefNo;
            TransId = param.TransId;
            BetId = param.BetId;
            CustId = param.CustId;

            MoveBetDay = moveBetsDate;
            BalanceUpDay = balanceUpDay;
        }

        public string RefNo { get; }

        public string TransId { get; }

        public string BetId { get; }

        public int CustId { get; }

        public DateTime MoveBetDay { get; }

        public DateTime BalanceUpDay { get; }

        public decimal Outstanding { get; set; }

        public override string GetSettingKey() => "CS_BetList_Parlay_TicketDetail";

        public override bool IsValid() => CustId > 0;
    }
}
