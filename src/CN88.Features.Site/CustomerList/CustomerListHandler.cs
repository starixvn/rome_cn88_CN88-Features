namespace CN88.Features.Site.Handlers
{
    using CN88.Features.CustomerList.Params;
    using CN88.Features.CustomerList.Services;
    using CN88.Features.Models;
    using CN88.Features.Site.CustomerList;
    using CN88.Features.Site.CustomerList.ViewModels;
    using MediatR;

    public class CustomerListHandler :
         IRequestHandler<IndexQuery, IndexViewModel>,
         IRequestHandler<CustomerListQuery, CustomerListViewModel>,
         IRequestHandler<CreditPopupQuery, CreditRangeViewModel>,
         IRequestHandler<CustomerStatusPopupQuery, StatusPopupViewModel>,
         IRequestHandler<CreditCommand, Result>,
         IRequestHandler<CustomerStatusCommand, Result>
    {
        private readonly CustomerListService service;

        public CustomerListHandler(CustomerListService service)
        {
            this.service = service;
        }

        public IndexViewModel Handle(IndexQuery message)
            => new IndexViewModel(message);

        public CustomerListViewModel Handle(CustomerListQuery message)
        {
            var param = new CustomerListParam(
                message.ViewerId,
                message.DownlineUsername,
                message.SearchRoleId,
                message.Username,
                message.Status,
                message.PageSize,
                message.PageIndex);
            var model = service.GetCustomerList(param);

            return new CustomerListViewModel(model);
        }

        public CreditRangeViewModel Handle(CreditPopupQuery message)
         => new CreditRangeViewModel(message, service.GetCreditRangeModel(message.CustId));

        public StatusPopupViewModel Handle(CustomerStatusPopupQuery message)
         => new StatusPopupViewModel(message);

        public Result Handle(CreditCommand message)
         => service.UpdateCredit(
               new UpdateCreditParam(
                   message.CustId,
                   message.CustLevelId,
                   message.CreditAmount,
                   message.ViewerId,
                   message.SubAccountId,
                   message.Language,
                   message.IP));

        public Result Handle(CustomerStatusCommand command)
            => service.UpdateCustomerStatus(new UpdateCustomerStatusParam(
               command.CustId,
               command.ViewerId,
               command.SubAccountId,
               command.UpdateStatusId,
               command.Language,
               command.IP,
               command.ViewerName));
    }
}