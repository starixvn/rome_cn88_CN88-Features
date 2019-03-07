namespace CN88.Features.Site.CustomerList.Requests
{
    using System;
    using CN88.Features.Models;
    using CN88.Features.Site.CustomerList.ViewModels;
    using CN88.Features.Site.Requests;
    using MediatR;

    public class MemberStatementQuery : BaseQuery, IRequest<MemberStatementViewModel>
    {     
        public MemberStatementQuery(IUser viewer) : base(viewer)
        {
        }

        public int SportType { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}