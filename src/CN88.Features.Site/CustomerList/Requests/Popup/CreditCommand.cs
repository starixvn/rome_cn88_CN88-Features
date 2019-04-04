namespace CN88.Features.Site.CustomerList
{
    using Features.Models;
    using MediatR;
    using Site.Requests;

    public class CreditCommand : BaseQuery, IRequest<Result>
    {
        public CreditCommand(IUser viewer)
            : base(viewer)
        {
            LanguageKey = viewer.LanguageKey;
        }

        public string LanguageKey { get; }

        public decimal CreditAmount { get; set; }
    }
}