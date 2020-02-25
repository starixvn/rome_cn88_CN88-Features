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

        public string UserSelectedPeriodId { get; set; }

        public bool IsValidReportRange(IEnumerable<AccountingPeriod> oldPeriods)
            => IsValidForNormalReportRange() || IsValidForOldReportRange(oldPeriods);

        private bool IsValidForNormalReportRange()
            => !IsOldReport && FromDate <= ToDate;

        private bool IsValidForOldReportRange(IEnumerable<AccountingPeriod> oldPeriods)
            => FromDate <= ToDate &&
                FromDate >= oldPeriods.Min(p => p.FromDate) &&
                ToDate <= oldPeriods.Max(p => p.ToDate);
    }
}