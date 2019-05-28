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

    public class WagerMonitoringViewModel : BetListViewModelBase
    {
        public WagerMonitoringViewModel(
            long minTransId,
            WagerMonitoringQuery message,
            BetListData betListData,
            IEnumerable<SelectListItemExt> sportGroups,
            ITicketHelper ticketHelper,
            IBetListCN88HtmlBuilder cn88Builder,
            IBetListHTMLBuilder builder,
            bool isMobile = false)
            : base(message, betListData, ticketHelper, cn88Builder, builder, isMobile, null)
        {
            MinTransId = minTransId;
            NoOfColumns = CalculateNumberOfColumns();

            FilterFormViewModel = new FilterFormViewModel(message);
            SportGroupSelectionViewModel = new SingleSelectionViewModel(sportGroups, message.SportGroup);

            IsShowWinLoss = false;
        }

        public long MinTransId { get; }

        public SingleSelectionViewModel SportGroupSelectionViewModel { get; }

        public FilterFormViewModel FilterFormViewModel { get; }

        public int IntervalTime => AppSettings.TimeToGetLastBetsInSeconds;

        public string WagerMonitoringPath => AppPathHelper.GetAppPath(AppSettings.FeaturesAppPath, "~/WagerMonitoring");

        public string FilterModalPath => AppPathHelper.GetAppPath(AppSettings.FeaturesAppPath, "~/WagerMonitoring/FilterModal");
    }
}