namespace CN88.Features.Site.BetLists.ViewModels.WagerMonitoring
{
    using System.Collections.Generic;
    using Alpha.Web.Helpers;
    using BetList.Core.Builders;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Features.BetLists.Models;
    using Features.Shared.Configurations;
    using Requests.WagerMonitoring;
    using Shared.ViewModels;
    using Shared.ViewModels.Components;

    public class WagerMonitoringViewModel : BetListTableViewModel
    {
        public WagerMonitoringViewModel(
            long minTransId,
            WagerMonitoringQuery query,
            BetListData betListData,
            IEnumerable<SelectListItemExt> sportGroups,
            ITicketHelper ticketHelper,
            IBetListCN88HtmlBuilder cn88Builder,
            IBetListHTMLBuilder builder,
            bool isMobile = false)
        {
            CustId = query.CustId;
            CustLevel = query.ActorLevel;
            ViewerIdOrViewerAdminId = query.ViewerIdOrViewerAdminId;
            ViewerLevelId = query.ViewerLevelId;
            Divisor = query.ActorLevel.IsHeadline() ? 100 : 1;
            NoOfColumns = CalculateNumberOfColumns();

            MinTransId = minTransId;
            BetListData = betListData;
            Builder = builder;
            CN88Builder = cn88Builder;
            TicketHelper = ticketHelper;

            FilterFormViewModel = new FilterFormViewModel(query);
            SportGroupSelectionViewModel = new SingleSelectionViewModel(sportGroups, query.SportGroup);

            IsMobile = isMobile;
            IsShowWinLoss = false;
            IsShowPayout = false;
        }

        public long MinTransId { get; }

        public SingleSelectionViewModel SportGroupSelectionViewModel { get; }

        public FilterFormViewModel FilterFormViewModel { get; }

        public int IntervalTime => AppSettings.TimeToGetLastBetsInSeconds;

        public string WagerMonitoringPath => AppPathHelper.GetAppPath(AppSettings.FeaturesAppPath, "~/WagerMonitoring");

        public string FilterModalPath => AppPathHelper.GetAppPath(AppSettings.FeaturesAppPath, "~/WagerMonitoring/FilterModal");
    }
}