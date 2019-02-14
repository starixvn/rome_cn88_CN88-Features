using System.Collections.Generic;
using System.Threading.Tasks;
using CN88.Features.Reports.Criteria;
using CN88.Features.Reports.Models;
using CN88.Features.Reports.Params;
using Fanex.Data.Repository;

namespace CN88.Features.Reports.Services
{
    public interface ReportService
    {
        Task<IEnumerable<ReportModel>> GetWagerReport(ReportParam param);

        Task<IEnumerable<ReportModel>> GetLegacyreport(ReportParam param);
    }

    public class ReportServiceImpl : ReportService
    {
        private readonly IDynamicRepository dynamicRepository;

        public ReportServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<ReportModel>> GetWagerReport(ReportParam param)
            => await dynamicRepository.FetchAsync<ReportModel>(
                new WagerReportCriteria(
                    param.ReportType,
                    param.FromDate,
                    param.ToDate,
                    param.SportTypeId,
                    param.BetTypeId));

        public async Task<IEnumerable<ReportModel>> GetLegacyreport(ReportParam param)
            => await dynamicRepository.FetchAsync<ReportModel>(
                new LegacyReportCriteria(
                    param.ReportType,
                    param.FromDate,
                    param.ToDate,
                    param.SportTypeId,
                    param.BetTypeId));
    }
}