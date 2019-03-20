namespace CN88.Features.Site.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Alpha.Web.Filters;
    using CustomerList;
    using CustomerList.Requests;
    using MediatR;
    using Shared.Controllers;
    using Web.Filters;

    [SessionState(SessionStateBehavior.ReadOnly), RestrictHeadlineFilter]
    public class CustomerListController : FeatureBaseController
    {
        private const string CustomerListPath = "~/CustomerList/Views/Index.cshtml";
        private const string StatusPopupPath = "~/CustomerList/Views/StatusPopup.cshtml";
        private const string CreditPopupPath = "~/CustomerList/Views/CreditPopup.cshtml";

        public CustomerListController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult> Index(IndexQuery message)
            => await ViewAsync(message, CustomerListPath);

        //[HttpGet]
        public async Task<JsonResult> GetCustomerList(CustomerListQuery message)
            => Json(await mediator.Send(message), JsonRequestBehavior.AllowGet);

        [HttpGet]
        public async Task<ActionResult> CreditPopup(CreditPopupQuery message)
           => await ViewAsync(message, CreditPopupPath);

        [HttpGet]
        public async Task<ActionResult> StatusPopup(CustomerStatusPopupQuery message)
             => await ViewAsync(message, StatusPopupPath);

        [HttpPost, AntiXsrfFilter]
        public ActionResult UpdateCredit(CreditCommand command)
            => JsonData(command);

        [HttpPost, AntiXsrfFilter]
        public ActionResult UpdateCustomerStatus(CustomerStatusCommand command)
            => JsonData(command);

        [HttpGet]
        public ActionResult Create(CreateCustomerQuery message)
        {
            var urlString = new List<string>
            {
                "~/CN88CustomerSetting/ViewCN88CustomerSetting/Create?",
                $"Language={message.Language}",
                $"&ProductId=1",
                $"&CustomerLevel={message.CustLevelId}",
                $"&CustomerId={message.ViewerId}",
                $"&IsReadOnly={!message.HasAddNewPermission}",
                $"&UplineId={message.ViewerId}",
                $"&ActorLevel={message.ViewerLevelId}"
            };

            return Redirect(string.Join(string.Empty, urlString));
        }

        [HttpGet]
        public async Task<ActionResult> OnlineMembers(OnlineMembersQuery message)
            => await ViewAsync(message, "~/CustomerList/Views/OnlineMembers.cshtml");

        public async Task<ActionResult> MemberStatement(MemberStatementQuery message)
            => await ViewAsync(message, "~/CustomerList/Views/MemberStatement.cshtml");
    }
}