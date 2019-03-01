namespace CN88.Features.Site.Reports.Requests
{
    using Features.Models;
    using Site.Requests;

    public class VerifyPasswordQuery : BaseQuery
    {
        public VerifyPasswordQuery(IUser viewer) : base(viewer)
        {
        }
    }
}