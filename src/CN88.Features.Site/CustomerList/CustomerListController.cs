namespace CN88.Features.Site.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Alpha.Web.Filters;
    using CN88.Web.Helpers;
    using CustomerList;
    using CustomerList.Requests;
    using MediatR;
    using Shared.Controllers;
    using Web.Filters;

    [SessionState(SessionStateBehavior.ReadOnly), RestrictHeadlineFilter]
    public class CustomerListController : FeatureBaseController
    {
        private readonly BrowserHelper browserHelper;

        public CustomerListController(IMediator mediator, BrowserHelper browserHelper) : base(mediator)
        {
            this.browserHelper = browserHelper;
        }

        [HttpGet]
        public async Task<ActionResult> Index(IndexQuery message)
            => browserHelper.IsMobile()
                ? await ViewAsync(message, "IndexMobile")
                : await ViewAsync(message);

        public async Task<JsonResult> GetCustomerList(CustomerListQuery message)
            => Json(await mediator.Send(message), JsonRequestBehavior.AllowGet);

        [HttpGet]
        public async Task<ActionResult> CreditPopup(CreditPopupQuery message)
           => await ViewAsync(message);

        [HttpGet]
        public async Task<ActionResult> StatusPopup(CustomerStatusPopupQuery message)
             => await ViewAsync(message);

        [HttpPost, AntiXsrfFilter]
        public ActionResult UpdateCredit(CreditCommand command)
            => JsonData(command);

        [HttpPost, AntiXsrfFilter]
        public ActionResult UpdateCustomerStatus(CustomerStatusCommand command)
            => JsonData(command);

        [HttpGet]
        public ActionResult Create(CreateCustomerQuery message)
        {
            var urlString = string.Concat(
                "~/CN88CustomerSetting/ViewCN88CustomerSetting/Create?",
                $"Language={message.Language}",
                $"&CustomerLevel={message.CustLevelId}",
                $"&CustomerId={message.ViewerId}",
                $"&IsReadOnly={!message.AccountManagementEditView}",
                $"&UplineId={message.ViewerId}",
                $"&ActorLevel={message.ViewerLevelId}",
                "&ProductId=1");

            return Redirect(urlString);
        }

        [HttpGet]
        public async Task<ActionResult> OnlineMembers(OnlineMembersQuery message)
            => await ViewAsync(message);

        public async Task<ActionResult> MemberStatement(MemberStatementQuery message)
            => await ViewAsync(message);
    }
}