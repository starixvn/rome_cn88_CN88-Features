namespace CN88.Features.Site.BetLists.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CN88.Features.Reports.Models;
    using Features.Models;
    using MediatR;
    using ViewModels;

    public class BetListOfReportQuery : BetListBaseQuery, IRequest<BetListOfReportViewModel>
    {
        public BetListOfReportQuery(IUser viewer) : base(viewer)
        {
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int UserSelectedSportGroupId { get; set; }

        public int UserSelectedBetTypeId { get; set; }

        public int UserSelectedLiveStatusId { get; set; }

        public int Type { get; set; }

        public int DisplayMode { get; set; }

        public string ReportMainActionPath { get; set; }

        public bool IsOldReport { get; set; }

        public bool IsSettlement { get; set; }

        public bool IsValidReportRange(IEnumerable<AccountingPeriod> periods)
            => periods.Any(p => FromDate >= p.FromDate && ToDate <= p.ToDate);
    }
}