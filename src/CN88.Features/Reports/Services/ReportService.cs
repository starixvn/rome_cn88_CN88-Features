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

        AssignedDownlinesCount GetAssignedDownlinesCount(int subAccId);
    }

    public class ReportServiceImpl : ReportService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly ReportDateService reportDateService;
        private readonly HeadlineService headlineService;

        public ReportServiceImpl(
            IDynamicRepository dynamicRepository,
            ReportDateService reportDateService,
            HeadlineService headlineService)
        {
            this.dynamicRepository = dynamicRepository;
            this.reportDateService = reportDateService;
            this.headlineService = headlineService;
        }

        public IEnumerable<ReportModel> GetReportData(ReportParam param)
        {
            var data = GetReportRawData(param);

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

        private IEnumerable<ReportModel> GetReportRawData(ReportParam param)
        {
            if (param.IsHeadline && !headlineService.IsCustomerBelonging(param.HeadlineId, param.CustId, param.FromDate, param.ToDate))
            {
                return Enumerable.Empty<ReportModel>();
            }

            DateTime moveBetsDay = DateTime.MinValue;
            DateTime archiveDate = DateTime.MinValue;

            if (param.ReportType != ReportTypeConstants.UnsettledWager)
            {
                moveBetsDay = reportDateService.GetMoveBetsDay();

                if (!param.IsHeadline)
                {
                    archiveDate = reportDateService.GetArchiveDate();
                }
            }

            return dynamicRepository.Fetch<ReportModel>(ReportCriteriaFactory.CreateCriteria(param, moveBetsDay, archiveDate));
        }

        public IEnumerable<PTHistory> GetPTHistories(int custid, DateTime fromDate, DateTime toDate)
            => dynamicRepository.Fetch<PTHistory>(
                new GetPTHistoryCriteria(
                    custid,
                    fromDate,
                    toDate,
                    AppSettings.IsTest));

        public AssignedDownlinesCount GetAssignedDownlinesCount(int subAccId)
            => dynamicRepository.Get<AssignedDownlinesCount>(new GetSubAccountAssignedDownlinesCount(subAccId));
    }
}