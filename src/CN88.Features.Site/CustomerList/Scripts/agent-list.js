var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var AgentList = /** @class */ (function (_super) {
    __extends(AgentList, _super);
    function AgentList($, statusComponent, numeral, pager) {
        var _this = _super.call(this, $, statusComponent, numeral, pager, "Agent") || this;
        _this.viewAgents = function () {
            _this.$.blockUI();
            _this.reportParam = {
                DownlineUsername: _this.$("#txt-viewed-master").val(),
                Username: _this.$username.val(),
                SearchRoleId: siteInfrastructure.roleId.agent,
                Status: _this.$dropdownStatus.find("option:selected").val(),
                PageSize: _this.reportParam.PageSize,
                PageIndex: _this.reportParam.PageIndex
            };
            if (_this.directDownline != siteInfrastructure.roleId.agent &&
                _this.reportParam.DownlineUsername == "") {
                _this.$errAgent.removeClass("display-none");
                _this.$.unblockUI();
                return;
            }
            _this.$errAgent.addClass("display-none");
            _this.loadData();
        };
        _this.renderAgentList = function (rs) {
            if (!rs.Customers) {
                return;
            }
            _this.$tblReport.removeClass("display-none");
            var html = "";
            var customerIndex = _this.reportParam.PageSize * (_this.reportParam.PageIndex - 1) + 1;
            if (rs.Customers.length == 0) {
                html = "<tr><td colspan='7' align='center' class='no-info'>" + _this.viewResources.NoInformationIsAvailable + "</td></tr>";
            }
            else {
                for (var i = 0; i < rs.Customers.length; i++, customerIndex++) {
                    var customer = rs.Customers[i];
                    html += "<tr class='row-customer " + _this.indexManager.getCustomerColor(customer) + "' cust-id='" + customer.CustId + "'>" +
                        "<td class='text-center'>" + customerIndex + "</td>" +
                        "<td class='view-downline' cust-id='" + customer.CustId + "'>" + customer.Username + "</td>" +
                        "<td>" + customer.Name + "</td>" +
                        "<td class='text-right update-credit' cust-id='" + customer.CustId + "' username='" + customer.Username + "'>" + _this.numeral(customer.Credit).format("###,0.00") + "</td>" +
                        "<td class='text-center'>" + customer.CreationDateStr + "</td>" +
                        "<td class='text-center update-status' cust-id='" + customer.CustId + "' username='" + customer.Username + "' current-status='" + customer.Status + "'>" + _this.indexManager.getStatusName(customer.Status) + "</td>" +
                        "<td class='text-center'>" +
                        "<span class='icon-edit' cust-id='" + customer.CustId + "' username='" + customer.Username + "' " + (_this.HasUpdatePermission ? "" : "disabled='disabled' title='" + _this.indexManager.viewResources.Disabled + "'") + "></span>" +
                        "<span class='icon-add' cust-id='" + customer.CustId + "' username='" + customer.Username + "' " + (_this.HasAddNewPermission ? "" : "disabled='disabled' title='" + _this.indexManager.viewResources.Disabled + "'") + "></span>" +
                        "</td>" +
                        "</tr>";
                }
            }
            _this.$tblReport.find("> tbody").empty().html(html);
            _this.$(window).trigger('resize.stickyTableHeaders');
            _super.prototype.bindReportEvents.call(_this, rs);
        };
        _this.viewAgentsOfMaster = function (custId, username) {
            _this.$viewedMaster.val(username);
            _this.$username.val("");
            _this.reportParam.PageIndex = 1;
            _this.reportParam.PageSize = 100;
            _this.statusComponent.changeSelection(siteInfrastructure.customerStatus.all);
            _this.viewAgents();
        };
        _this.initAutoComplete = function () {
            var queryUsernameURL = _this.usernameAutoSuggestionPath + "?" +
                _this.$.param({ isDir: 1, issub: 0, ismember: 0 });
            initAutoComplete(_this.$viewedMaster[0], queryUsernameURL);
            _this.$output.css("top", _this.$viewedMaster.offset().top + 30);
            _this.$output.css("left", _this.$viewedMaster.offset().left);
        };
        _this.reloadState = function (reportParam) {
            _this.$("#txt-viewed-master").val(reportParam.DownlineUsername);
            _this.$username.val(reportParam.Username);
            _this.$dropdownStatus.val(reportParam.Status);
            _this.reportParam.PageSize = reportParam.PageSize;
            _this.reportParam.PageIndex = reportParam.PageIndex;
            _this.$("#PageIndexAgent").val(reportParam.PageIndex);
            _this.$("#PageSizeAgent").val(reportParam.PageIndex);
            _this.viewAgents();
        };
        return _this;
    }
    AgentList.prototype.init = function (viewResources, indexManager) {
        _super.prototype.init.call(this, viewResources, indexManager, this.renderAgentList, siteInfrastructure.roleId.agent, indexManager.viewAgent, indexManager.addMemberForAgent);
        this.cacheDOMElements();
        var self = this;
        this.$("#btn-submit-agent").off("click").on("click", function () {
            self.reportParam.PageSize = 100;
            self.reportParam.PageIndex = 1;
            self.viewAgents();
        });
        if (this.directDownline >= siteInfrastructure.roleId.master) {
            this.initAutoComplete();
        }
    };
    AgentList.prototype.cacheDOMElements = function () {
        _super.prototype.cacheDOMElements.call(this);
        this.$viewedMaster = this.$("#txt-viewed-master");
        this.directDownline = parseInt(this.$("#directDownline").val());
        this.$errAgent = this.$("#msg-err-agent");
        this.usernameAutoSuggestionPath = this.$("#usernameAutoSuggestionPath").val();
        this.$output = this.$("#output");
    };
    return AgentList;
}(BaseList));
//# sourceMappingURL=agent-list.js.map