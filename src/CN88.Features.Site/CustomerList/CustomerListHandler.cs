namespace CN88.Features.Site.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using CN88.Features.CustomerList.Params;
    using CN88.Features.CustomerList.Services;
    using CN88.Features.Models;
    using CN88.Features.Site.CustomerList;
    using CN88.Features.Site.CustomerList.ViewModels;
    using CustomerList.Requests;
    using Features.CustomerList;
    using Features.Shared.AppResources;
    using Features.Shared.Models;
    using Features.Shared.Services;
    using MediatR;

    public class CustomerListHandler :
         IRequestHandler<IndexQuery, IndexViewModel>,
         IRequestHandler<CustomerListQuery, CustomerListViewModel>,
         IRequestHandler<CreditPopupQuery, CreditRangeViewModel>,
         IRequestHandler<CustomerStatusPopupQuery, StatusPopupViewModel>,
         IRequestHandler<CreditCommand, Result>,
         IRequestHandler<CustomerStatusCommand, Result>,
         IRequestHandler<OnlineMembersQuery, OnlineMemberViewModel>,
        IRequestHandler<MemberStatementQuery, MemberStatementViewModel>
    {
        private readonly CustomerListService service;
        private readonly MemberStatementService statementService;
        private readonly SportGroupService sportService;

        public CustomerListHandler(
            CustomerListService service,
            MemberStatementService statementService,
            SportGroupService sportService)
        {
            this.sportService = sportService;
            this.service = service;
            this.statementService = statementService;
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

            return new CustomerListViewModel(
                model,
                message.CustId.Equals(0) ? message.ViewerId : message.CustId,
                message.Username,
                message.DownlineUsername);
        }

        public CreditRangeViewModel Handle(CreditPopupQuery message)
         => new CreditRangeViewModel(message, service.GetCreditRangeModel(message.CustId, message.CustLevel.IsMemberLevel()));

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

        public OnlineMemberViewModel Handle(OnlineMembersQuery message)
        {
            var param = new OnlineMembersParam(message.ViewerId, message.PageSize, message.PageIndex);
            var onlineMembers = service.OnlineMembers(param);

            return new OnlineMemberViewModel(onlineMembers, message, param.TotalRow);
        }

        public MemberStatementViewModel Handle(MemberStatementQuery message)
        {
            var param = new MemberStatementParam
            {
                CustId = message.CustId,
                FromDate = message.FromDate,
                ToDate = message.ToDate,
                SportType = message.SportType
            };
            var statements = statementService.Get(param);
            var sportGroups = sportService.GetSportGroups();
            return new MemberStatementViewModel(
                statements,
                message,
                BuildSportTypeSelectListItems(sportGroups, message.SportType));
        }

        private IEnumerable<SelectListItem> BuildSportTypeSelectListItems(
            IEnumerable<SportGroup> sportTypes,
            int userSelectedSportGroupId)
        {
            var items = sportTypes.Select(i => new SelectListItem
            {
                Text = i.DisplayName,
                Value = i.Id.ToString(),
                Selected = i.Id == userSelectedSportGroupId
            }).ToList();

            items.Insert(0, new SelectListItem
            {
                Text = SharedResources.AllSports,
                Value = "0",
                Selected = userSelectedSportGroupId == 0
            });

            return items;
        }
    }
}