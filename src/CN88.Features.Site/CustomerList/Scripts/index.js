var CustomerList = /** @class */ (function () {
    function CustomerList($, statusComponent, numeral, pager, topWindow) {
        var _this = this;
        this.reportParam = {
            DownlineUsername: null,
            Username: "",
            SearchRoleId: 0,
            Status: 0,
            PageSize: 100,
            PageIndex: 1
        };
        this.viewResources = {
            NoInformationIsAvailable: '',
            Open: '',
            Closed: '',
            Suspended: '',
            Page: '',
            Of: '',
            Pager_DisplayItems: '',
            Disabled: ''
        };
        this.init = function () {
            _this.cacheDOMElements();
            _this.masterList.init(_this.viewResources, _this);
            _this.agentList.init(_this.viewResources, _this);
            _this.memberList.init(_this.viewResources, _this);
            _this.$navTabs.find("li[level='" + _this.directDownline + "']").addClass("active");
            _this.$(".tab-content[level='" + _this.directDownline + "']").removeClass("display-none");
            var self = _this;
            _this.$(".lnk-changetab").click(function () {
                var $this = self.$(this);
                self.$navTabs.find("> li").removeClass("active");
                $this.addClass("active");
                self.changeTab($this.attr("level"));
            });
            _this.$(".form-inline").bind("keypress", function (e) {
                if (e.keyCode == 13) {
                    return false;
                }
            });
            if (_this.reportParam.SearchRoleId > 0) {
                _this.$(".lnk-changetab[level='" + _this.reportParam.SearchRoleId + "']").click();
                switch (_this.reportParam.SearchRoleId) {
                    case siteInfrastructure.roleId.master:
                        _this.masterList.reloadState(_this.reportParam);
                        break;
                    case siteInfrastructure.roleId.agent:
                        _this.agentList.reloadState(_this.reportParam);
                        break;
                    case siteInfrastructure.roleId.member:
                        _this.memberList.reloadState(_this.reportParam);
                        break;
                }
            }
            else {
                switch (parseInt(_this.directDownline)) {
                    case siteInfrastructure.roleId.master:
                        _this.masterList.viewList();
                        break;
                    case siteInfrastructure.roleId.agent:
                        _this.agentList.viewAgents();
                        break;
                    case siteInfrastructure.roleId.member:
                        _this.memberList.viewMembers();
                        break;
                }
            }
        };
        this.cacheDOMElements = function () {
            _this.directDownline = _this.$("#directDownline").val();
            _this.$navTabs = _this.$("#nav-tabs");
            _this.viewResources.NoInformationIsAvailable = _this.$("#NoInformationIsAvailable").val();
            _this.viewResources.Open = _this.$("#Open").val();
            _this.viewResources.Closed = _this.$("#Closed").val();
            _this.viewResources.Suspended = _this.$("#Suspended").val();
            _this.viewResources.Page = _this.$("#Page").val();
            _this.viewResources.Of = _this.$("#of").val();
            _this.viewResources.Pager_DisplayItems = _this.$("#Pager_DisplayItems").val();
            _this.viewResources.Disabled = _this.$("#Disabled").val();
            _this.EditCustomerLink = _this.$("#EditCustomerLink").val();
            _this.CreateCustomerLink = _this.$("#CreateCustomerLink").val();
            _this.CurrentLink = _this.$("#CurrentLink").val();
            _this.HasUpdatePermission = _this.$("#HasUpdatePermission").val() == "1";
            _this.HasAddNewPermission = _this.$("#HasAddNewPermission").val() == "1";
            _this.Language = _this.$("#Language").val();
            _this.ViewerId = _this.$("#ViewerId").val();
            _this.ActorLevel = _this.$("#ActorLevel").val();
            _this.reportParam = {
                DownlineUsername: _this.$("#DownlineUsername").val(),
                Username: _this.$("#Username").val(),
                SearchRoleId: parseInt(_this.$("#SearchRoleId").val()),
                Status: parseInt(_this.$("#Status").val()),
                PageSize: parseInt(_this.$("#PageSize").val()),
                PageIndex: 1
            };
        };
        this.changeTab = function (level) {
            this.$(".tab-content").addClass("display-none");
            this.$(".tab-content[level='" + level + "']").removeClass("display-none");
            switch (parseInt(level)) {
                case siteInfrastructure.roleId.agent:
                    this.agentList.initAutoComplete();
                    break;
                case siteInfrastructure.roleId.member:
                    this.memberList.initAutoComplete();
                    break;
            }
        };
        this.getCustomerColor = function (customer) {
            if (customer.Status == siteInfrastructure.customerStatus.open) {
                return "row-open";
            }
            if (customer.Status == siteInfrastructure.customerStatus.suspended) {
                return "row-suspended";
            }
            if (customer.Status == siteInfrastructure.customerStatus.closed) {
                return "row-closed reason-" + customer.CloseReason;
            }
            return "";
        };
        this.updateCredit = function (custId, username, custLevelId) {
            var url = _this.$("#creditPopupUrl").val() + "?custId=" + custId + "&custLevelId=" + custLevelId, caption = _this.$("#UpdateCredit").val();
            _this.topWindow.popupManager.open(url, caption + " - " + username, 550, 250);
        };
        this.updateStatus = function (custId, username, updateStatusId) {
            var url = _this.$("#statusPopupUrl").val() + "?custId=" + custId + "&UpdateStatusId=" + updateStatusId, popupTitle = _this.$("#UpdateStatus").val() + " - " + username;
            var width = _this.topWindow.displayTextWidth(popupTitle, _this.$(".col-username").css("fontSize"));
            _this.topWindow.popupManager.open(url, popupTitle, width + 150, 250);
        };
        this.updateAccount = function (custId, username, custLevelId, reportParam) {
            _this.storeCurrentPage(reportParam);
            _this.topWindow.showCN88CancelButton = true;
            window.location.href = _this.EditCustomerLink + "?" + _this.$.param({
                Language: _this.Language,
                ProductId: 1,
                CustomerLevel: custLevelId,
                CustomerId: custId,
                IsReadOnly: !_this.HasUpdatePermission,
                UplineId: _this.ViewerId,
                ActorLevel: _this.ActorLevel
            });
        };
        this.addAgentForMaster = function (custId, username, custLevelId, reportParam) {
            _this.storeCurrentPage(reportParam);
            _this.topWindow.showCN88CancelButton = true;
            window.location.href = _this.CreateCustomerLink + "?" + _this.$.param({
                Language: _this.Language,
                ProductId: 1,
                CustomerLevel: custLevelId,
                CustomerId: custId,
                IsReadOnly: !_this.HasAddNewPermission,
                UplineId: custId,
                ActorLevel: _this.ActorLevel
            });
        };
        this.addMemberForAgent = function (custId, username, custLevelId, reportParam) {
            _this.storeCurrentPage(reportParam);
            _this.topWindow.showCN88CancelButton = true;
            window.location.href = _this.CreateCustomerLink + "?" + _this.$.param({
                Language: _this.Language,
                ProductId: 1,
                CustomerLevel: custLevelId,
                CustomerId: custId,
                IsReadOnly: !_this.HasAddNewPermission,
                IsCreateNew: true,
                UplineId: custId,
                ActorLevel: _this.ActorLevel
            });
        };
        this.getStatusName = function (status) {
            if (status == siteInfrastructure.customerStatus.open) {
                return _this.viewResources.Open;
            }
            if (status == siteInfrastructure.customerStatus.closed) {
                return _this.viewResources.Closed;
            }
            if (status == siteInfrastructure.customerStatus.suspended) {
                return _this.viewResources.Suspended;
            }
            return "";
        };
        this.viewMaster = function (custId, username) {
            _this.$(".lnk-changetab[level='" + siteInfrastructure.roleId.agent + "']").click();
            _this.agentList.viewAgentsOfMaster(custId, username);
        };
        this.viewAgent = function (custId, username) {
            _this.$(".lnk-changetab[level='" + siteInfrastructure.roleId.member + "']").click();
            _this.memberList.viewMembersOfAgent(custId, username);
        };
        this.reloadCreditValue = function (custId, newValue) {
            _this.$(".update-credit[cust-id='" + custId + "']").html(_this.numeral(newValue).format("###,0.00"));
        };
        this.reloadStatusValue = function (custId, newValue) {
            var column = _this.$(".update-status[cust-id='" + custId + "']");
            column.html(_this.getStatusName(newValue));
            column.attr("current-status", newValue);
            _this.$(".row-customer[cust-id='" + custId + "']")
                .removeClass()
                .addClass("row-customer " + _this.reloadStatusColor(newValue));
        };
        this.reloadStatusColor = function (status) {
            if (status == siteInfrastructure.customerStatus.open) {
                return "row-open";
            }
            if (status == siteInfrastructure.customerStatus.suspended) {
                return "row-suspended";
            }
            return "row-closed reason-" + siteInfrastructure.closeReasonConstant.Upline;
        };
        this.storeCurrentPage = function (reportParam) {
            var currentPage = _this.CurrentLink + "?" + _this.$.param(reportParam);
            var $currentPage = _this.topWindow.$("body #reloadCN88CustomerListCallBackUrl");
            if ($currentPage.length == 0) {
                _this.topWindow.$("body").append("<input id='reloadCN88CustomerListCallBackUrl' type='hidden' value='' />");
            }
            _this.topWindow.$("body #reloadCN88CustomerListCallBackUrl").val(currentPage);
        };
        this.$ = $;
        this.topWindow = topWindow;
        this.statusComponent = statusComponent;
        this.numeral = numeral;
        this.pager = pager;
        this.masterList = new MasterList($, new SingleSelectionDropdownComponent($), numeral, new PagerComponent($));
        this.agentList = new AgentList($, new SingleSelectionDropdownComponent($), numeral, new PagerComponent($));
        this.memberList = new MemberList($, new SingleSelectionDropdownComponent($), numeral, new PagerComponent($));
    }
    return CustomerList;
}());
;
var customerList = new CustomerList($, new SingleSelectionDropdownComponent($), numeral, new PagerComponent($), window.top);
$(function () {
    customerList.init();
});
//# sourceMappingURL=index.js.map