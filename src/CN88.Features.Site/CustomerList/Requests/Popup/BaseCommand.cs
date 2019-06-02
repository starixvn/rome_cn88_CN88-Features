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

        // TO DO
        public override bool IsValid() => base.IsValid() && !IsInternal;
    }
}