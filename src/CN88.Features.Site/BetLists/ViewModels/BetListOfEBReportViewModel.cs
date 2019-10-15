namespace CN88.Features.Site.BetLists.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using BetList.Core.Builders;
    using CN88.Features.Shared.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Features.BetLists.Models;
    using Features.Reports.AppResources;
    using Features.Reports.Constants;
    using Features.Shared.AppResources;
    using Features.Shared.Constants;
    using Requests;
    using Shared.ViewModels;
    using Shared.ViewModels.Components;

    public class BetListOfEBReportViewModel : BetListViewModelBase
    {
        public BetListOfEBReportViewModel(
            BetListOfEBReportQuery message,
            BetListData betListData,
            ITicketHelper ticketHelper,
            IBetListCN88HtmlBuilder cn88Builder,
            IBetListHTMLBuilder builder,
            IEnumerable<SportGroup> sportGroups,
            IEnumerable<SportGroup> allSportGroups,
            IEnumerable<SelectListItemExt> betTypes,
            bool isMobile = false,
            string[] excludingTicketStatus = null)
            : base(message, betListData, ticketHelper, cn88Builder, builder, isMobile, excludingTicketStatus)
        {
            Query = message;

            BetTypeGroup = message.UserSelectedBetTypeId;
            ReportType = ReportTypes[message.Type];
            ReportTitle = ReportResources.ResourceManager.GetString(ReportTypeConstants.ReportTypes[ReportType]);
            SportGroups = sportGroups.Count() == allSportGroups.Count() ? SharedResources.AllSports : string.Join(", ", sportGroups.Select(i => i.DisplayName));

            BetTypeSelectionViewModel = new SingleSelectionViewModel(betTypes, message.UserSelectedBetTypeId);
            LiveStatusSelectionViewModel = new SingleSelectionViewModel(GetLiveStatusList(message), message.UserSelectedLiveStatusId);
        }

        public BetListOfEBReportQuery Query { get; }

        public SingleSelectionViewModel BetTypeSelectionViewModel { get; }

        public SingleSelectionViewModel LiveStatusSelectionViewModel { get; }

        public string SportGroups { get; }

        public string ReportTitle { get; private set; }

        public int ReportType { get; private set; }

        protected override int GetMaxColumn() => 11;

        private static IEnumerable<SelectListItemExt> GetLiveStatusList(BetListOfEBReportQuery message)
            => LiveStatusConstants.LiveStatuses
                            .Select(s => new SelectListItemExt
                            {
                                Text = SharedResources.ResourceManager.GetString(s.Value),
                                Value = s.Key.ToString(),
                                Selected = s.Key == message.UserSelectedLiveStatusId
                            });
    }
}