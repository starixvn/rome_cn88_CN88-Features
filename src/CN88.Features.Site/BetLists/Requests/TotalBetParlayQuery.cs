namespace CN88.Features.Site.BetLists.Requests
{
    using Features.Models;
    using MediatR;
    using ViewModels;

    public class TotalBetParlayQuery : BetListBaseQuery, IRequest<TotalBetParlayViewModel>
    {
        private const long maxStakeLimit = 10000000000;

        public TotalBetParlayQuery(IUser viewer) : base(viewer)
        {
            CustId = viewer.Id;
            CustName = viewer.Name;
            CustLevelId = viewer.LevelId;
        }

        public int SportGroupId { get; set; }

        public int ViewMode { get; set; }

        public int StakeMode { get; set; } = 1;

        public decimal StakeLimit { get; set; }

        public override bool IsValid()
            => base.IsValid() && StakeLimit < maxStakeLimit;
    }
}