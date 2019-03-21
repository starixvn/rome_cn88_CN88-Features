namespace CN88.Features.Site.CustomerList.Requests
{
    using CN88.Features.Models;
    using CN88.Features.Shared.Configurations;
    using CN88.Features.Site.CustomerList.ViewModels;
    using CN88.Features.Site.Requests;
    using MediatR;

    public class MemberStatementQuery : BaseQuery, IRequest<MemberStatementViewModel>
    {
        public MemberStatementQuery(IUser viewer) : base(viewer)
        {
        }

        public int UserSelectedSportGroupId { get; set; }

        public int UserSelectedFromDate { get; set; } = DefaultFromDate;

        public int UserSelectedToDate { get; set; }

        private static int DefaultFromDate = AppSettings.MemberStatementOfLatestDay - 1;
    }
}