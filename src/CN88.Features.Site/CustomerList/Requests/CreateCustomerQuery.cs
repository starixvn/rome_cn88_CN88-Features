namespace CN88.Features.Site.CustomerList
{
    using CN88.Features.Models;
    using CN88.Features.Site.Requests;

    public class CreateCustomerQuery : BaseQuery
    {
        public CreateCustomerQuery(IUser viewer) : base(viewer)
        {
            HasAddNewPermission = viewer.HasAddNewPermission;
        }

        public bool HasAddNewPermission { get; }
    }
}