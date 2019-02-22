namespace CN88.Features.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Constants;
    using Criteria;
    using Extensions;
    using Fanex.Caching;
    using Fanex.Common;
    using Fanex.Data.Repository;
    using Models;
    using Params;
    using RawData;
    using Shared.Services;

    public interface ReportService
    {
        IEnumerable<WinLossModel> GetWinLoss(ReportParam param);

        Task<IEnumerable<AccountingPeriodGroup>> GetAccountingPeriodGroups();

        Task<AccountingPeriod> GetCurrentPeriod();

        Task<SettlementSummaryReport> GetSettlementSummaryReport();
    }

    public class ReportServiceImpl : ReportService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly ReportDateService reportDateService;
        private readonly CacheRefService cacheRefService;

        public ReportServiceImpl(
            IDynamicRepository dynamicRepository,
            ReportDateService reportDateService,
            CacheRefService cacheRefService)
        {
            this.dynamicRepository = dynamicRepository;
            this.reportDateService = reportDateService;
            this.cacheRefService = cacheRefService;
        }

        public IEnumerable<WinLossModel> GetWinLoss(ReportParam param)
            => dynamicRepository.Fetch<WinLossModel>(CreateReportCriteria(param));

        public async Task<IEnumerable<AccountingPeriodGroup>> GetAccountingPeriodGroups()
        {
            var periods = await GetAccountingPeriods();
            var groups = periods
                .OrderBy(p => p.PeriodId)
                .GroupBy(p => p.PeriodId.ToString());
            var result = new List<AccountingPeriodGroup>();

            foreach (var group in groups)
            {
                var ps = new List<AccountingPeriod>();

                foreach (var p in group)
                {
                    ps.Add(p);
                }

                result.Add(new AccountingPeriodGroup(group.Key, ps.OrderByDescending(p => p.Year).ToList()));
            }

            return result;
        }

        public async Task<AccountingPeriod> GetCurrentPeriod()
            => (await GetAccountingPeriods()).FirstOrDefault(p => p.IsCurrentPeriod);

        public async Task<SettlementSummaryReport> GetSettlementSummaryReport()
        {
            var rawData = await dynamicRepository.FetchAsync<SportSettlementByDateModel>(new GetSettlementSummaryReportCriteria());

            var sportGroups = rawData
                .Select(d => d.SportTypeGroup)
                .Distinct()
                .OrderBy(s => Converter.ToInt(s));
            var name = DateTime.Now.AddDays(-1).ToShortDateFormat();
            var yesterday = new SettlementByGroup(
                0,
                $"{AppResources.ReportResources.Yesterday}({name})",
                GetSettlementByDate(name, rawData, sportGroups));

            name = DateTime.Now.ToShortDateFormat();
            var today = new SettlementByGroup(
                0,
                $"{AppResources.ReportResources.Today}({name})",
                GetSettlementByDate(name, rawData, sportGroups));

            var result = new SettlementSummaryReport(sportGroups.Select(s => cacheRefService.GetSportName(Converter.ToInt(s))).ToList(), yesterday, today);

            return result;
        }

        private async Task<IEnumerable<AccountingPeriod>> GetAccountingPeriods()
            => await ConfiguredCacheService.GetOrSetAsync(
                "CN88.Features.Reports.Services.ReportServiceImpl.AccountingPeriods",
                () => dynamicRepository.Fetch<AccountingPeriod>(new GetAccountingPeriodsCriteria()),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));

        private IList<SettlementModel> GetSettlementByDate(
            string name,
            IEnumerable<SportSettlementByDateModel> rawData,
            IEnumerable<string> sportGroups)
        {
            var result = new List<SettlementModel>();
            foreach (var sportGroup in sportGroups)
            {
                var data = rawData.FirstOrDefault(d =>
                Converter.To<DateTime>(d.EventDate).ToShortDateFormat() == name && sportGroup == d.SportTypeGroup);
                var sport = data == null ?
                    new SettlementModel(0, sportGroup, 0, 0) :
                    new SettlementModel(0, sportGroup, data.IncompletedEvent, data.CompletedEvent);

                result.Add(sport);
            }

            return result;
        }

        private ICriteria CreateReportCriteria(ReportParam param)
        {
            var balanceUpDay = reportDateService.GetBalanceUpDay();
            var moveBetsDay = reportDateService.GetMoveBetsDay();

            if (param.ReportType == ReportTypeConstants.SettledWager)
            {
                return new GetWinLossReportCriteria(
                    param.CustId,
                    param.IsOwner,
                    param.FromDate,
                    param.ToDate,
                    balanceUpDay,
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
                    balanceUpDay,
                    moveBetsDay,
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
                balanceUpDay,
                moveBetsDay,
                param.SportGroupId,
                param.BetTypeId,
                param.IsLive,
                param.ReportType,
                param.ReportLevel);
        }
    }
}