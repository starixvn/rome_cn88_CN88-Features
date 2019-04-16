namespace CN88.Features.Site.SubAccount.Requests
{
    using System.Collections.Generic;
    using Features.Models;
    using MediatR;

    public class NewCommand : UpdateCommand, IRequest<Result>
    {
        public NewCommand(IUser viewer)
            : base(viewer)
        {
        }

        public string Suffix1 { get; set; }

        public string Suffix2 { get; set; }

        public string Password { get; set; }

        public IEnumerable<string> InvalidPasswords
        {
            get
            {
                yield return ViewerName;
            }
        }
    }
}