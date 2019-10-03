namespace CN88.Features.BetLists.Criteria.Parlay
{
    using Fanex.Data.Repository;
    using Params;

    public class GetParlaySubTicketsCriteria : CriteriaBase
    {
        public GetParlaySubTicketsCriteria(GetParlaySubTicketsParam param)
        {
            TransId = param.TransId;
            CustId = param.CustId;
            HeadlineId = param.HeadlineId;
        }

        public string TransId { get; }

        public int CustId { get; }

        public int HeadlineId { get; }

        public override string GetSettingKey() => "CS_BetList_Parlay_GetSubTickets";

        public override bool IsValid() => CustId > 0;
    }
}