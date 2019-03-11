namespace CN88.Features.BetLists.Criteria
{
    using System;
    using Constants;
    using Fanex.Data.Repository;
    using Params;

    public static class BetListOfReportCriteriaFactory
    {
        public static CriteriaBase CreateCriteria(BetListOfReportParam param, DateTime moveBetsDate, bool isBackup)
        {
            if (isBackup)
            {
                switch (param.Type)
                {
                    case BetListTypeConstant.Settled:
                        return new OldSettledBetListCriteria(param);
                    case BetListTypeConstant.Unsettled:
                        return new UnsettledBetListCriteria(param);
                    case BetListTypeConstant.Cancelled:
                        return new OldCancelledBetListCriteria(param);
                    case BetListTypeConstant.Abnormal:
                        return new OldAbnormalBetListCriteria(param);
                    default:
                        return new SettledBetListCriteria(param, moveBetsDate);
                }
            }

            switch (param.Type)
            {
                case BetListTypeConstant.Settled:
                    return new SettledBetListCriteria(param, moveBetsDate);
                case BetListTypeConstant.Unsettled:
                    return new UnsettledBetListCriteria(param);
                case BetListTypeConstant.Cancelled:
                    return new CancelledBetListCriteria(param, moveBetsDate);
                case BetListTypeConstant.Abnormal:
                    return new AbnormalBetListCriteria(param, moveBetsDate);
                default:
                    return new SettledBetListCriteria(param, moveBetsDate);
            }
        }
    }
}
