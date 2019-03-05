namespace CN88.Features.Site.Reports.Requests
{
    using Features.Models;
    using MediatR;
    using Site.Requests;
    using ViewModels;

    public class VerifyPasswordQuery : BaseQuery, IRequest<VerifyPasswordViewModel>
    {
        public VerifyPasswordQuery(IUser viewer) : base(viewer)
        {
        }
    }
}