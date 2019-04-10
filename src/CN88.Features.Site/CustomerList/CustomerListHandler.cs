namespace CN88.Features.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
                model.UplineId,
                message.Username,
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
                message.IP,
                message.IsInternal));

        public Result Handle(CustomerStatusCommand message)
            => service.UpdateCustomerStatus(new UpdateCustomerStatusParam(
               message.CustId,
               message.ViewerId,
               message.SubAccountId,
               message.UpdateStatusId,
               message.LanguageKey,
               message.IP,
               message.ViewerName,
               message.IsInternal));

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
                FromDate = message.UserSelectedFromDate,
                ToDate = message.UserSelectedToDate,
                SportType = message.UserSelectedSportGroupId
            };
            var statements = statementService.Get(param);
            var sportGroups = sportService.GetSportGroups();

            var availableDate = statementService.GetAvailableDate();
            var fromDates = availableDate;
            var toDates = availableDate;

            return new MemberStatementViewModel(
                statements,
                message,
                BuildSportTypeSelectListItems(sportGroups, message.UserSelectedSportGroupId),
                BuildDateTimeSelectListItems(fromDates, message.UserSelectedFromDate),
                BuildDateTimeSelectListItems(fromDates, message.UserSelectedToDate));
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