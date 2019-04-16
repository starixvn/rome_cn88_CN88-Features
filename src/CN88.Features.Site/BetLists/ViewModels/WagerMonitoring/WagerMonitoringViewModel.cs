namespace CN88.Features.Site.BetLists.ViewModels.WagerMonitoring
{
    using System.Collections.Generic;
    using Alpha.Web.Helpers;
    using BetList.Core.Builders;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Features.BetLists.Models;
    using Features.Shared.Configurations;
    using Features.Shared.Models;
    using Requests.WagerMonitoring;
    using Shared.ViewModels;
    using Shared.ViewModels.Components;

    public class WagerMonitoringViewModel
    {
        public WagerMonitoringViewModel(
            WagerMonitoringQuery query,
            IEnumerable<TicketDto> tickets,
            IEnumerable<SelectListItemExt> sportGroups,
            ITicketHelper ticketHelper,
            IBetListCN88HtmlBuilder cn88Builder,
            IBetListHTMLBuilder builder)
        {
            CustLevel = query.ActorLevel;
            Divisor = query.ActorLevel.IsHeadline() ? 100 : 1;
            NoOfColumns = CustLevel.IsSuperLevel() || CustLevel.IsHeadline() ? 9 : CustLevel.IsMasterLevel() ? 7 : 6;

            Tickets = tickets;
            Builder = builder;
            CN88Builder = cn88Builder;
            TicketHelper = ticketHelper;

            FilterFormViewModel = new FilterFormViewModel(query);
            SportGroupSelectionViewModel = new SingleSelectionViewModel(sportGroups, query.SportGroup);
        }


        public int Divisor { get; }

        public int NoOfColumns { get; }

        public UserLevel CustLevel { get; }

        public IEnumerable<TicketDto> Tickets { get; }

        public ITicketHelper TicketHelper { get; }

        public IBetListHTMLBuilder Builder { get; }

        public IBetListCN88HtmlBuilder CN88Builder { get; }

        public SingleSelectionViewModel SportGroupSelectionViewModel { get; }

        public FilterFormViewModel FilterFormViewModel { get; }

        public int PageSize => AppSettings.WagerMonitoringPageSize;

        public int IntervalTime => AppSettings.TimeToGetLastBetsInSeconds;

        public string WagerMonitoringPath => AppPathHelper.GetAppPath(AppSettings.FeaturesAppPath, "~/WagerMonitoring");

        public string FilterFormPath => AppPathHelper.GetAppPath(AppSettings.FeaturesAppPath, "~/WagerMonitoring/FilterForm");

        public string FilterModalPath => AppPathHelper.GetAppPath(AppSettings.FeaturesAppPath, "~/WagerMonitoring/FilterModal");
    }
}