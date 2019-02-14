namespace CN88.Features.Site.CustomerList
{
    using CN88.Features.Models;
    using CN88.Features.Site.Requests;
    using MediatR;

    public class CreditPopupQuery : BaseQuery, IRequest<CreditRangeViewModel>
    {
        public CreditPopupQuery(IUser viewer) : base(viewer)
        {
        }
    }
}