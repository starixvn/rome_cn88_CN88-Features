namespace CN88.Features.Site.CustomerList
{
    using Features.Models;
    using Site.Requests;

    public class BaseCommand : BaseQuery
    {
        public BaseCommand(IUser viewer)
            : base(viewer)
        {
        }

        public override bool IsValid() => base.IsValid() && !IsInternal;

        public override string ErrorMessage => string.Empty;
    }
}