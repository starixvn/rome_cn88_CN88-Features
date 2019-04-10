namespace CN88.Features.Site.CustomerList
{
    using Features.Models;
    using MediatR;
    using Site.Requests;

    public class CustomerStatusCommand : BaseQuery, IRequest<Result>
    {
        public CustomerStatusCommand(IUser viewer)
            : base(viewer)
        {
            LanguageKey = viewer.LanguageKey;
            IsInternal = viewer.IsInternal;
        }

        public bool IsInternal { get; }

        public string LanguageKey { get; }

        public byte UpdateStatusId { get; set; }
    }
}