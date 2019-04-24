namespace CN88.Features.Site.CustomerList
{
    using Features.Models;
    using Site.Requests;

    public class CreateCustomerQuery : BaseQuery
    {
        public CreateCustomerQuery(IUser viewer)
            : base(viewer)
        {
            AccountManagementEditView = viewer.AccountManagementEditView;
        }

        public bool AccountManagementEditView { get; }
    }
}