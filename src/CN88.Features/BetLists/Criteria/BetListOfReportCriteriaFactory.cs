namespace CN88.Features.BetLists.Criteria
{
    using System;
    using Constants;
    using Fanex.Data.Repository;
    using Params;

    public static class BetListOfReportCriteriaFactory
    {
        public static CriteriaBase CreateCriteria(
            BetListOfReportParam param,
            DateTime moveBetsDate,
            DateTime fromDate,
            DateTime toDate,
            bool isBackup)
        {
            var newParam = new BetListOfReportParam(
                param.CustId,
                fromDate,
                toDate,
                param.SportTypeGroup,
                param.BetTypeGroup,
                param.LiveIndicator,
                param.Type,
                param.LimitedDate);

            if (isBackup)
            {
                switch (param.Type)
                {
                    case BetListTypeConstant.Settled:
                        return new OldSettledBetListCriteria(newParam);
                    case BetListTypeConstant.Unsettled:
                        return new UnsettledBetListCriteria(newParam);
                    case BetListTypeConstant.Cancelled:
                        return new OldCancelledBetListCriteria(newParam);
                    case BetListTypeConstant.Abnormal:
                        return new OldAbnormalBetListCriteria(newParam);
                    default:
                        return new SettledBetListCriteria(newParam, moveBetsDate);
                }
            }

            switch (param.Type)
            {
                case BetListTypeConstant.Settled:
                    return new SettledBetListCriteria(newParam, moveBetsDate);
                case BetListTypeConstant.Unsettled:
                    return new UnsettledBetListCriteria(newParam);
                case BetListTypeConstant.Cancelled:
                    return new CancelledBetListCriteria(newParam, moveBetsDate);
                case BetListTypeConstant.Abnormal:
                    return new AbnormalBetListCriteria(newParam, moveBetsDate);
                default:
                    return new SettledBetListCriteria(newParam, moveBetsDate);
            }
        }
    }
}
