namespace CN88.Features.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Criteria;
    using Fanex.Data.Repository;
    using Models;
    using Params;
    using Shared.Configurations;
    using Shared.Extensions;
    using Shared.Services;

    public interface ReportService
    {
        IEnumerable<ReportModel> GetReportData(ReportParam param);

        IEnumerable<PTHistory> GetPTHistories(int custid, DateTime fromDate, DateTime toDate);
    }

    public class ReportServiceImpl : ReportService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly ReportDateService reportDateService;

        public ReportServiceImpl(
            IDynamicRepository dynamicRepository,
            ReportDateService reportDateService)
        {
            this.dynamicRepository = dynamicRepository;
            this.reportDateService = reportDateService;
        }

        public IEnumerable<ReportModel> GetReportData(ReportParam param)
        {
            var data = dynamicRepository.Fetch<ReportModel>(CreateReportCriteria(param));

            if (data != null && data.Any())
            {
                var custIds = data.Select(d => d.CustId).Distinct().JoinWithComma();

                var ptSettings = dynamicRepository.Fetch<PTSetting>(
                    new GetCustomerPTSettingCriteria(param.ReportLevel.Value, custIds, AppSettings.IsTest));

                var ptSettingGroups = ptSettings?.GroupBy(pt => pt.CustId);

                if (ptSettingGroups == null || !ptSettingGroups.Any())
                {
                    return data;
                }

                foreach (var item in data)
                {
                    var group = ptSettingGroups
                        .FirstOrDefault(g => g.Key == item.CustId)
                        ?.OrderBy(g => g.SportType);

                    if (group != null)
                    {
                        item.ExtraPT = group.Select(g => g.ExtraPT);
                        item.SuperPT = group.Select(g => g.SuperPT);
                        item.MasterPT = group.Select(g => g.MasterPT);
                        item.AgentPT = group.Select(g => g.AgentPT);
                    }
                }
            }

            return data;
        }

        private ICriteria CreateReportCriteria(ReportParam param)
        {
            var moveBetsDay = reportDateService.GetMoveBetsDay();
            var archiveDate = reportDateService.GetArchiveDate();

            if (param.ReportType == ReportTypeConstants.SettledWager)
            {
                return new GetWinLossReportCriteria(
                    param.CustId,
                    param.IsOwner,
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
                    param.IsOwner,
                    param.FromDate,
                    param.ToDate,
                    param.SportGroupId,
                    param.BetTypeId,
                    param.IsLive,
                    param.ReportLevel);
            }

            return new GetCancelledReportCriteria(
                param.CustId,
                param.IsOwner,
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

        public IEnumerable<PTHistory> GetPTHistories(int custid, DateTime fromDate, DateTime toDate)
            => dynamicRepository.Fetch<PTHistory>(
                new GetPTHistoryCriteria(
                    custid,
                    fromDate,
                    toDate,
                    AppSettings.IsTest));
    }
}