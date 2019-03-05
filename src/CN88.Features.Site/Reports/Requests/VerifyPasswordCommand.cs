namespace CN88.Features.Site.Reports.Requests
{
    using Features.Models;
    using MediatR;
    using Site.Requests;

    public class VerifyPasswordCommand : BaseQuery, IRequest<Result>
    {
        public VerifyPasswordCommand(IUser viewer)
            : base(viewer)
        {
        }

        public string Password { get; set; }
    }
}