namespace CN88.Features.Site.BetLists.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;
    using Features.BetLists.Models;
    using Features.BetLists.Resources;
    using Helpers;
    using Requests;

    public class ParlayDetailViewModel
    {
        private readonly Dictionary<string, int> statusOrder = new Dictionary<string, int>
        {
            { string.Empty, 0 },
            { BetStatus.Won, 1 },
            { BetStatus.Lose, 2 },
            { BetStatus.Draw, 3 },
            { BetStatus.Void, 4 },
            { BetStatus.Reject, 5 },
            { BetStatus.Refund, 6 }
        };

        public ParlayDetailViewModel(
               GetParlayDetailQuery query,
               ParlayDetailReportModel parlayDetailData,
               ITicketHelper ticketHelper,
               bool isExportingExcel = false)
        {
            Divisor = query.Divisor;

            IsDataTooHuge = parlayDetailData.ParlayDetails.Count() > 5000;
            Query = query;

            Title = BetListsResources.SystemParlay;

            var allowInitializingData = isExportingExcel || !IsDataTooHuge;

            if (allowInitializingData)
            {
                ParlayDetailWrappers = new List<ParlayDetailWrapper>(parlayDetailData.ParlayDetails
                        .OrderBy(i => statusOrder[i.Status.ToLowerInvariant()])
                        .Select(d => new ParlayDetailWrapper(d, parlayDetailData.MatchDetails, ticketHelper, Divisor)));
                ParlayDetailWrapperGroups = ParlayDetailWrappers.GroupBy(w => w.WinLostDate);

                OutStanding = BetListHelpers.Stake(parlayDetailData.Outstanding / Divisor);
                TotalWinLost = ConvertByBetType.Winloss(parlayDetailData.ParlayDetails.Sum(d => d.WinLost + d.PlayerComm) / Divisor);
                IsRunningTicket = !parlayDetailData.ParlayDetails.Any();
            }
        }

        public GetParlayDetailQuery Query { get; }

        public IEnumerable<ParlayDetailWrapper> ParlayDetailWrappers { get; }

        public IEnumerable<IGrouping<string, ParlayDetailWrapper>> ParlayDetailWrapperGroups { get; }

        public string TotalWinLost { get; }

        public string OutStanding { get; }

        public string Title { get; }

        public bool IsDataTooHuge { get; }

        public bool IsRunningTicket { get; }

        public int Divisor { get; }

        public string HugeDataLabel
            => string.Format(BetListsResources.CombinationResultTooHuge, BuildComboParlayDetailExcelPath(Query));

        public string SubTotalWinLost(IEnumerable<ParlayDetailWrapper> group)
            => ConvertByBetType.Winloss(group.Sum(item => item.ParlayDetail.WinLost + item.ParlayDetail.PlayerComm) / Divisor);

        private static string BuildComboParlayDetailExcelPath(GetParlayDetailQuery query)
        {
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

            return urlHelper.Action("ParlayDetailExcel", "BetLists", query);
        }
    }
}
