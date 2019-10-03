namespace CN88.Features.Reports.Criteria
{
    using System;
    using CN88.Features.Reports.Constants;
    using CN88.Features.Reports.Params;
    using Fanex.Data.Repository;

    public static class ReportCriteriaFactory
    {
        public static ICriteria CreateCriteria(ReportParam param, DateTime moveBetsDay, DateTime archiveDate)
            => param.IsHeadline
                ? CreateHeadlineReportCriteria(param, moveBetsDay)
                : CreateReportCriteria(param, moveBetsDay, archiveDate);

        private static ICriteria CreateHeadlineReportCriteria(ReportParam param, DateTime moveBetsDay)
        {
            if (param.ReportType == ReportTypeConstants.SettledWager)
            {
                return new GetHeadlineWinLossReportCriteria(
                    param.HeadlineId,
                    param.CustId,
                    param.FromDate,
                    param.ToDate,
                    moveBetsDay,
                    param.SportGroupId,
                    param.BetTypeId,
                    param.IsLive,
                    param.ReportLevel);
            }

            if (param.ReportType == ReportTypeConstants.UnsettledWager)
            {
                return new GetHeadlineOutstandingReportCriteria(
                    param.HeadlineId,
                    param.CustId,
                    param.FromDate,
                    param.ToDate,
                    param.SportGroupId,
                    param.BetTypeId,
                    param.IsLive,
                    param.ReportLevel);
            }

            return new GetHeadlineCancelledReportCriteria(
                param.HeadlineId,
                param.CustId,
                param.FromDate,
                param.ToDate,
                moveBetsDay,
                param.SportGroupId,
                param.BetTypeId,
                param.IsLive,
                param.ReportType,
                param.ReportLevel);
        }

        private static ICriteria CreateReportCriteria(ReportParam param, DateTime moveBetsDay, DateTime archiveDate)
        {
            if (param.ReportType == ReportTypeConstants.SettledWager)
            {
                return new GetWinLossReportCriteria(
                    param.CustId,
                    param.SubAccId,
                    param.IsOwner,
                    param.IsAllDownline,
                    param.FromDate,
                    param.ToDate,
                    archiveDate,
                    moveBetsDay,
                    param.SportGroupId,
                    param.BetTypeId,
                    param.IsLive,
                    param.ReportLevel);
            }

            if (param.ReportType == ReportTypeConstants.UnsettledWager)
            {
                return new GetOutstandingReportCriteria(
                    param.CustId,
                    param.SubAccId,
                    param.IsOwner,
                    param.IsAllDownline,
                    param.FromDate,
                    param.ToDate,
                    param.SportGroupId,
                    param.BetTypeId,
                    param.IsLive,
                    param.ReportLevel);
            }

            return new GetCancelledReportCriteria(
                param.CustId,
                param.SubAccId,
                param.IsOwner,
                param.IsAllDownline,
                param.FromDate,
                param.ToDate,
                archiveDate,
                moveBetsDay,
                param.SportGroupId,
                param.BetTypeId,
                param.IsLive,
                param.ReportType,
                param.ReportLevel);
        }
    }
}
