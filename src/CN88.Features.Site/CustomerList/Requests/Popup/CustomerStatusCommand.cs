namespace CN88.Features.Site.CustomerList
{
    using CN88.Features.CustomerList;
    using CN88.Features.CustomerList.Resources;
    using Features.Models;
    using MediatR;

    public class CustomerStatusCommand : BaseCommand, IRequest<Result>
    {
        public CustomerStatusCommand(IUser viewer) : base(viewer)
        {
        }

        private bool IsActivedUpline => UplineStatusId.Equals(CustomerStatus.Active);

        public byte UplineStatusId { get; set; } = CustomerStatus.Active;

        public byte OldStatusId { get; set; }

        public byte NewStatusId { get; set; }

        public override bool IsValid() => base.IsValid() && IsActivedUpline && !IsInternal;

        public override string ErrorMessage => 
            IsActivedUpline ? string.Empty :
            CustomerListResources.UpdateStatusFailedCauseOfDirectUplineNotActive;
    }
}