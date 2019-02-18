namespace CN88.Features.Site.Controllers
{
    using Alpha.Web.Filters;
    using CN88.Features.Site.CustomerList;
    using MediatR;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class CustomerListController : SiteBaseController
    {
        private const string CustomerListPath = "~/CustomerList/Views/Index.cshtml";

        public CustomerListController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet]
        public async Task<ActionResult> Index(IndexQuery message)
            => View(CustomerListPath, await mediator.Send(message));

        [HttpGet]
        public JsonResult GetCustomerList(CustomerListQuery message)
            => Json(mediator.Send(message), JsonRequestBehavior.AllowGet);

        [HttpGet]
        public ActionResult CreditPopup(CreditPopupQuery message)
           => View(mediator.Send(message));

        [HttpGet]
        public ActionResult StatusPopup(CustomerStatusPopupQuery message)
            => View(mediator.Send(message));

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
    }
}