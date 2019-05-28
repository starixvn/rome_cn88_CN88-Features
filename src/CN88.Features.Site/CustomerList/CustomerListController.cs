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
        public CustomerListController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult> Index(IndexQuery message)
            => await ViewAsync(message);

        [HttpGet]
        public async Task<JsonResult> GetCustomerList(CustomerListQuery message)
            => Json(await mediator.Send(message), JsonRequestBehavior.AllowGet);

        [HttpGet]
        public async Task<ActionResult> CreditPopup(CreditPopupQuery message)
           => await ViewAsync(message);

        [HttpGet]
        public async Task<ActionResult> StatusPopup(CustomerStatusPopupQuery message)
             => await ViewAsync(message);

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> UpdateCredit(CreditCommand command)
            => await JsonDataAsync(command);

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> UpdateUnlock(UnlockCommand command)
            => await JsonDataAsync(command);

        [HttpPost, AntiXsrfFilter]
        public async Task<ActionResult> UpdateCustomerStatus(CustomerStatusCommand command)
            => await JsonDataAsync(command);

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