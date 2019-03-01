namespace CN88.Features.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria;
    using Fanex.Common;
    using Fanex.Data.Repository;
    using Models;
    using RawData;
    using Shared.Extensions;
    using Shared.Services;

    public interface SettlementSummaryService
    {
        SettlementSummaryReport GetSettlementSummaryReport();
    }

    public class SettlementSummaryServiceImpl : SettlementSummaryService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly CacheRefService cacheRefService;

        public SettlementSummaryServiceImpl(
            IDynamicRepository dynamicRepository,
            CacheRefService cacheRefService)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheRefService = cacheRefService;
        }

        public SettlementSummaryReport GetSettlementSummaryReport()
        {
            var rawData = dynamicRepository.Fetch<SportSettlementByDateModel>(new GetSettlementSummaryReportCriteria());

            var sportGroups = rawData
                .Select(d => d.SportTypeGroup)
                .Distinct()
                .OrderBy(s => Converter.ToInt(s));
            var name = DateTime.Now.AddDays(-1).ToShortDate();
            var yesterday = new SettlementByGroup(
                0,
                $"{AppResources.ReportResources.Yesterday}({name})",
                GetSettlementByDate(name, rawData, sportGroups));

            name = DateTime.Now.ToShortDate();
            var today = new SettlementByGroup(
                0,
                $"{AppResources.ReportResources.Today}({name})",
                GetSettlementByDate(name, rawData, sportGroups));

            var result = new SettlementSummaryReport(sportGroups.Select(s => cacheRefService.GetSportName(Converter.ToInt(s))).ToList(), yesterday, today);

            return result;
        }

        private IList<SettlementModel> GetSettlementByDate(
            string name,
            IEnumerable<SportSettlementByDateModel> rawData,
            IEnumerable<string> sportGroups)
        {
            var result = new List<SettlementModel>();
            foreach (var sportGroup in sportGroups)
            {
                var data = rawData.FirstOrDefault(d =>
                Converter.To<DateTime>(d.EventDate).ToShortDate() == name && sportGroup == d.SportTypeGroup);
                var sport = data == null ?
                    new SettlementModel(0, sportGroup, 0, 0) :
                    new SettlementModel(0, sportGroup, data.IncompletedEvent, data.CompletedEvent);

                result.Add(sport);
            }

            return result;
        }
    }
}