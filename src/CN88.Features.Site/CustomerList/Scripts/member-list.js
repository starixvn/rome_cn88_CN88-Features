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
var MemberList = /** @class */ (function (_super) {
    __extends(MemberList, _super);
    function MemberList($, statusComponent, numeral, pager) {
        var _this = _super.call(this, $, statusComponent, numeral, pager, "Member") || this;
        _this.viewMembers = function () {
            _this.$.blockUI();
            _this.reportParam = {
                DownlineUsername: _this.$("#txt-viewed-agent").val(),
                Username: _this.$username.val(),
                SearchRoleId: siteInfrastructure.roleId.member,
                Status: _this.$dropdownStatus.find("option:selected").val(),
                PageSize: _this.reportParam.PageSize,
                PageIndex: _this.reportParam.PageIndex
            };
            if (_this.directDownline != siteInfrastructure.roleId.agent &&
                _this.reportParam.DownlineUsername == "") {
                _this.$errMember.removeClass("display-none");
                _this.$.unblockUI();
                return;
            }
            _this.$errMember.addClass("display-none");
            _this.loadData();
        };
        _this.renderMemberList = function (rs) {
            if (!rs.Customers) {
                return;
            }
            _this.$tblReport.removeClass("display-none");
            var html = "";
            var customerIndex = _this.reportParam.PageSize * (_this.reportParam.PageIndex - 1) + 1;
            if (rs.Customers.length == 0) {
                html = "<tr><td colspan='8' align='center' class='no-info'>" + _this.viewResources.NoInformationIsAvailable + "</td></tr>";
            }
            else {
                for (var i = 0; i < rs.Customers.length; i++, customerIndex++) {
                    var customer = rs.Customers[i];
                    html += "<tr class='row-customer " + _this.indexManager.getCustomerColor(customer) + "' cust-id='" + customer.CustId + "'>" +
                        "<td class='text-center'>" + customerIndex + "</td>" +
                        "<td>" + customer.Username + "</td>" +
                        "<td>" + customer.Name + "</td>" +
                        "<td class='text-center'>" + customer.Group + "</td>" +
                        "<td class='text-right update-credit' cust-id='" + customer.CustId + "' username='" + customer.Username + "'>" + _this.numeral(customer.Credit).format("###,0.00") + "</td>" +
                        "<td class='text-center'>" + customer.CreationDateStr + "</td>" +
                        "<td class='text-center update-status' cust-id='" + customer.CustId + "' username='" + customer.Username + "' current-status='" + customer.Status + "'>" + _this.indexManager.getStatusName(customer.Status) + "</td>" +
                        "<td class='text-center'>" +
                        "<span class='icon-edit' cust-id='" + customer.CustId + "' username='" + customer.Username + "' " + (_this.HasUpdatePermission ? "" : "disabled='disabled' title='" + _this.indexManager.viewResources.Disabled + "'") + "></span>" +
                        "</td>" +
                        "</tr>";
                }
            }
            _this.$tblReport.find("> tbody").empty().html(html);
            _this.$(window).trigger('resize.stickyTableHeaders');
            _super.prototype.bindReportEvents.call(_this, rs);
        };
        _this.viewMembersOfAgent = function (custId, username) {
            _this.$viewedAgent.val(username);
            _this.$username.val("");
            _this.reportParam.PageIndex = 1;
            _this.reportParam.PageSize = 100;
            _this.statusComponent.changeSelection(siteInfrastructure.customerStatus.all);
            _this.viewMembers();
        };
        _this.initAutoComplete = function () {
            var queryUsernameURL = _this.usernameAutoSuggestionPath + "?" +
                _this.$.param({ isDir: _this.directDownline == siteInfrastructure.roleId.agent ? 1 : 0, issub: 0, ismember: 1 });
            initAutoComplete(_this.$viewedAgent[0], queryUsernameURL);
            _this.$output.css("top", _this.$viewedAgent.offset().top + 30);
            _this.$output.css("left", _this.$viewedAgent.offset().left);
        };
        _this.reloadState = function (reportParam) {
            _this.$("#txt-viewed-agent").val(reportParam.DownlineUsername);
            _this.$username.val(reportParam.Username);
            _this.$dropdownStatus.val(reportParam.Status);
            _this.reportParam.PageSize = reportParam.PageSize;
            _this.reportParam.PageIndex = reportParam.PageIndex;
            _this.$("#PageIndexMember").val(reportParam.PageIndex);
            _this.$("#PageSizeMember").val(reportParam.PageIndex);
            _this.viewMembers();
        };
        return _this;
    }
    MemberList.prototype.init = function (viewResources, indexManager) {
        _super.prototype.init.call(this, viewResources, indexManager, this.renderMemberList, siteInfrastructure.roleId.member, null, null);
        this.cacheDOMElements();
        var self = this;
        this.$("#btn-submit-member").off("click").on("click", function () {
            self.reportParam.PageSize = 100;
            self.reportParam.PageIndex = 1;
            self.viewMembers();
        });
        if (this.directDownline >= siteInfrastructure.roleId.agent) {
            this.initAutoComplete();
        }
    };
    MemberList.prototype.cacheDOMElements = function () {
        _super.prototype.cacheDOMElements.call(this);
        this.$viewedAgent = this.$("#txt-viewed-agent");
        this.directDownline = parseInt(this.$("#directDownline").val());
        this.$errMember = this.$("#msg-err-member");
        this.usernameAutoSuggestionPath = this.$("#usernameAutoSuggestionPath").val();
        this.$output = this.$("#output");
    };
    return MemberList;
}(BaseList));
//# sourceMappingURL=member-list.js.map