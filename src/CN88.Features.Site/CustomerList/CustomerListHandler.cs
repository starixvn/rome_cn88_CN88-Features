namespace CN88.Features.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CN88.Web.Helpers;
    using CustomerList;
    using CustomerList.Requests;
    using CustomerList.ViewModels;
    using Extensions;
    using Features.CustomerList;
    using Features.CustomerList.Params;
    using Features.CustomerList.Services;
    using Features.Models;
    using Features.Shared.AppResources;
    using Features.Shared.Models;
    using Features.Shared.Services;
    using MediatR;
    using Shared.ViewModels;

    public class CustomerListHandler :
        IRequestHandler<IndexQuery, IndexViewModel>,
        IRequestHandler<CustomerListQuery, CustomerListViewModel>,
        IRequestHandler<CreditPopupQuery, CreditRangeViewModel>,
        IRequestHandler<CustomerStatusPopupQuery, StatusPopupViewModel>,
        IRequestHandler<CreditCommand, Result>,
         IRequestHandler<UnlockCustomerCommand, Result>,
        IRequestHandler<CustomerStatusCommand, Result>,
        IRequestHandler<OnlineMembersQuery, OnlineMemberViewModel>,
         IRequestHandler<MemberStatementQuery, MemberStatementViewModel>
    {
        private readonly CustomerListService service;
        private readonly MemberStatementService statementService;
        private readonly SportGroupService sportService;
        private readonly BrowserHelper browserHelper;

        public CustomerListHandler(
            CustomerListService service,
            MemberStatementService statementService,
            SportGroupService sportService,
            BrowserHelper browserHelper)
        {
            this.sportService = sportService;
            this.service = service;
            this.statementService = statementService;
            this.browserHelper = browserHelper;
        }

        public IndexViewModel Handle(IndexQuery message)
            => new IndexViewModel(message, browserHelper);

        public CustomerListViewModel Handle(CustomerListQuery message)
        {
            var param = new CustomerListParam(
                message.ViewerId,
                message.SubAccountId,
                message.DownlineUsername,
                message.SearchRoleId,
                message.Username,
                message.Status,
                message.PageSize,
                message.PageIndex);
            var model = service.GetCustomerList(param);

            return new CustomerListViewModel(
                model,
                model.UplineId,
                message.Username,
                model.UplineStatus,
                message.DownlineUsername);
        }

        public CreditRangeViewModel Handle(CreditPopupQuery message)
         => new CreditRangeViewModel(message, service.GetCreditRangeModel(message.CustId, message.CustLevel.IsMemberLevel()));

        public StatusPopupViewModel Handle(CustomerStatusPopupQuery message)
         => new StatusPopupViewModel(message);

        public Result Handle(CreditCommand message)
         => service.UpdateCredit(new UpdateCreditParam(
                message.CustId,
                message.CustLevelId,
                message.CreditAmount,
                message.ViewerId,
                message.SubAccountId,
                message.LanguageKey,
                message.IP));

        public Result Handle(UnlockCustomerCommand message)
         => service.UnlockCustomer(new UnlockCustomerParam(
                message.CustId,
                message.ViewerId,
                message.SubAccountId,
                message.LanguageKey,
                message.IP));

        public Result Handle(CustomerStatusCommand message)
            => service.UpdateCustomerStatus(new UpdateCustomerStatusParam(
               message.CustId,
               message.ViewerId,
               message.SubAccountId,
               message.OldStatusId,
               message.NewStatusId,
               message.LanguageKey,
               message.IP,
               message.ViewerName));

        public OnlineMemberViewModel Handle(OnlineMembersQuery message)
        {
            var param = new OnlineMembersParam(
                message.ViewerId,
                message.SubAccountId,
                message.IsAllDownline,
                message.PageSize,
                message.PageIndex);

            var onlineMembers = service.OnlineMembers(param);

            return new OnlineMemberViewModel(onlineMembers, message, param.TotalRow);
        }

        public MemberStatementViewModel Handle(MemberStatementQuery message)
        {
            var param = new MemberStatementParam(
                message.CustId,
                message.UserSelectedSportGroupId,
                message.UserSelectedFromDate,
                message.UserSelectedToDate);

            var statements = statementService.Get(param);
            var sportGroups = sportService.GetSportGroups();

            var fromDates = statementService.GetAvailableDate();

            return new MemberStatementViewModel(
                statements,
                message,
                BuildSportTypeSelectListItems(sportGroups, message.UserSelectedSportGroupId),
                BuildDateTimeSelectListItems(fromDates, message.UserSelectedFromDate),
                BuildDateTimeSelectListItems(fromDates, message.UserSelectedToDate),
                browserHelper.IsMobile());
        }

        private IEnumerable<SelectListItemExt> BuildSportTypeSelectListItems(
            IEnumerable<SportGroup> sportTypes,
            int userSelectedSportGroupId)
        {
            var items = sportTypes.Select(i => new SelectListItemExt
            {
                Text = i.DisplayName,
                Value = i.Id.ToString(),
                Selected = i.Id == userSelectedSportGroupId
            }).ToList();

            items.Insert(0, new SelectListItemExt
            {
                Text = SharedResources.AllSports,
                Value = "0",
                Selected = userSelectedSportGroupId == 0
            });

            return items;
        }

        private IEnumerable<SelectListItemExt> BuildDateTimeSelectListItems(
            Dictionary<int, DateTime> dateTimes,
            int userSelectedSportGroupId)
        {
            var items = dateTimes.Select(i => new SelectListItemExt
            {
                Text = i.Value.ToShortDateFormat(),
                Value = i.Key.ToString(),
                Selected = i.Key == userSelectedSportGroupId
            }).OrderByDescending(i => i.Value);

            return items;
        }
    }
}
