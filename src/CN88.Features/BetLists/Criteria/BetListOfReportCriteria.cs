namespace CN88.Features.BetLists.Criteria
{
    using System;
    using Constants;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Params;

    public class BetListOfReportCriteria : CriteriaBase
    {
        public BetListOfReportCriteria(BetListOfReportParam param, DateTime moveBetsDate)
        {
            CustId = param.CustId;
            FromDate = param.FromDate;
            ToDate = param.ToDate;
            SportTypeGroup = param.SportTypeGroup;
            BetTypeGroup = param.BetTypeGroup;
            LiveIndicator = param.LiveIndicator;
            Type = param.Type;

            MoveBetsDay = moveBetsDate;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime MoveBetsDay { get; }

        public int SportTypeGroup { get; }

        public int BetTypeGroup { get; }

        public int LiveIndicator { get; }

        [SpParam(Ignored = true)]
        public int Type { get; }

        public override string GetSettingKey()
        {
            switch (Type)
            {
                case BetListTypeConstant.Settled:
                    return "CS_BetList_WinLoss_Mem";
                case BetListTypeConstant.OldSettled:
                    return "CS_BetList_WinLoss_Mem_Old";
                case BetListTypeConstant.Unsettled:
                    return "CS_BetList_Unsettled_Mem";
                case BetListTypeConstant.Cancelled:
                    return "CS_BetList_CancelledBet_Mem";
                case BetListTypeConstant.OldCancelled:
                    return "CS_BetList_CancelledBet_Mem_Old";
                case BetListTypeConstant.Abnormal:
                    return "CS_BetList_AbnormalBet_Mem";
                case BetListTypeConstant.OldAbnormal:
                    return "CS_BetList_AbnormalBet_Mem_Old";
                default:
                    return "CS_BetList_WinLoss_Mem";
            }
        }

        public override bool IsValid() => CustId > 0;
    }
}
