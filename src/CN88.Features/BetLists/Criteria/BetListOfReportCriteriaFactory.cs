namespace CN88.Features.BetLists.Criteria
{
    using System;
    using Constants;
    using Fanex.Data.Repository;
    using Params;

    public static class BetListOfReportCriteriaFactory
    {
        public static CriteriaBase CreateCriteria(BetListOfReportParam param, DateTime moveBetsDate)
        {
            switch (param.Type)
            {
                case BetListTypeConstant.Settled:
                    return new SettledBetListCriteria(param, moveBetsDate);
                case BetListTypeConstant.OldSettled:
                    return new OldSettledBetListCriteria(param);
                case BetListTypeConstant.Unsettled:
                    return new UnsettledBetListCriteria(param);
                case BetListTypeConstant.Cancelled:
                    return new CancelledBetListCriteria(param, moveBetsDate);
                case BetListTypeConstant.OldCancelled:
                    return new OldCancelledBetListCriteria(param);
                case BetListTypeConstant.Abnormal:
                    return new AbnormalBetListCriteria(param, moveBetsDate);
                case BetListTypeConstant.OldAbnormal:
                    return new OldAbnormalBetListCriteria(param);
                default:
                    return new SettledBetListCriteria(param, moveBetsDate);
            }
        }
    }
}
