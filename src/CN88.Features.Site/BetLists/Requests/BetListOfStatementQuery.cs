namespace CN88.Features.Site.BetLists.Requests
{
    using System;
    using Features.Models;
    using MediatR;
    using ViewModels;

    public class BetListOfStatementQuery : BetListBaseQuery, IRequest<BetListOfStatementViewModel>
    {
        public BetListOfStatementQuery(IUser viewer) : base(viewer)
        {
        }

        public DateTime Date { get; set; }

        public int SportTypeGroup { get; set; }
    }
}