"use strict";
var SingleSelectionDropdownOption = (function () {
    function SingleSelectionDropdownOption(dropdownId, inputId, onChange) {
        if (dropdownId === void 0) { dropdownId = "dropdown-single"; }
        if (inputId === void 0) { inputId = "UserSelectedId"; }
        if (onChange === void 0) { onChange = null; }
        this.dropdownId = dropdownId;
        this.inputId = inputId;
        this.onChange = onChange;
    }
    return SingleSelectionDropdownOption;
}());
var SingleSelectionDropdownComponent = (function () {
    function SingleSelectionDropdownComponent($) {
        var _this = this;
        this.options = new SingleSelectionDropdownOption();
        this.render = function (options) {
            if (options === void 0) { options = new SingleSelectionDropdownOption(); }
            _this.options = _this.$.extend({}, _this.options, options);
            var $dropdown = _this.$("#" + _this.options.dropdownId);
            _this.$dropdown = $dropdown;
            var $hiddenInput = _this.$("#" + _this.options.inputId);
            _this.$hiddenInput = $hiddenInput;
            var self = _this;
            _this.selectedValue = self.getSelectedItem();
            _this.$dropdown.multiselect({
                multiple: false,
                maxHeight: 300,
                numberDisplayed: 1,
                includeSelectAllOption: false,
                onChange: _this.onchange
            });
        };
        this.getSelectedItem = function () {
            var selected = [];
            var selectedOptions = _this.$dropdown.find("option:selected");
            var this$ = _this.$;
            selectedOptions.each(function () {
                selected.push([this$(this).val()]);
            });
            return selected.length ? selected[0].toString() : "";
        };
        this.updateSelectedValue = function () {
            _this.$hiddenInput.val(_this.selectedValue);
        };
        this.onchange = function () {
            _this.selectedValue = _this.getSelectedItem();
            if (_this.options.onChange) {
                _this.options.onChange();
            }
        };
        this.changeSelection = function (selection) {
            _this.$dropdown.val(selection);
            _this.$hiddenInput.val(selection);
            _this.$dropdown.multiselect("select", [selection]);
            _this.$dropdown.multiselect("refresh");
        };
        this.$ = $;
    }
    return SingleSelectionDropdownComponent;
}());
//# sourceMappingURL=single-selection-dropdown.js.map
"use strict";
var PagerOption = (function () {
    function PagerOption(onSelectPage, pagerId, formId, pageIndexId, pageSizeId, pageList) {
        if (onSelectPage === void 0) { onSelectPage = null; }
        if (pagerId === void 0) { pagerId = "report-pager"; }
        if (formId === void 0) { formId = "report-form"; }
        if (pageIndexId === void 0) { pageIndexId = "PageIndex"; }
        if (pageSizeId === void 0) { pageSizeId = "PageSize"; }
        if (pageList === void 0) { pageList = [50, 100, 200, 500]; }
        this.onSelectPage = onSelectPage;
        this.pagerId = pagerId;
        this.formId = formId;
        this.pageIndexId = pageIndexId;
        this.pageSizeId = pageSizeId;
        this.pageList = pageList;
    }
    return PagerOption;
}());
var PagerComponent = (function () {
    function PagerComponent($) {
        var _this = this;
        this.pageList = [];
        this.render = function (options) {
            if (options === void 0) { options = _this.options; }
            var self = _this;
            var pagerOption = _this.$.extend({}, _this.options, options);
            var pager = _this.$("#" + pagerOption.pagerId);
            _this.$reportForm = _this.$("#" + pagerOption.formId);
            _this.totalRecords = pager.data("totalrecords");
            _this.pageSize = pager.data("pagesize");
            _this.pageIndex = pager.data("pageindex");
            _this.$pageIndex =
                _this.$("<input type=\"hidden\" id=\"" + pagerOption.pageIndexId + "\" name=\"" + pagerOption.pageIndexId + "\" value=\"" + _this.pageIndex + "\" />");
            _this.$pageSize =
                _this.$("<input type=\"hidden\" id=\"" + pagerOption.pageSizeId + "\" name=\"" + pagerOption.pageSizeId + "\" value=\"" + _this.pageSize + "\" />");
            _this.pageList = pagerOption.pageList;
            _this.$reportForm.append(_this.$pageIndex).append(_this.$pageSize);
            if (_this.isShow()) {
                pager.pagination({
                    pageList: _this.pageList,
                    total: _this.totalRecords,
                    pageSize: _this.pageSize,
                    pageNumber: _this.pageIndex,
                    showRefresh: false,
                    onSelectPage: function (pageNumber, pageSize) {
                        self.updateHiddenFields(pageNumber, pageSize);
                        if (pagerOption.onSelectPage && typeof (pagerOption.onSelectPage) == "function") {
                            pagerOption.onSelectPage();
                        }
                    },
                    beforePageText: pager.data("labelpage"),
                    afterPageText: pager.data("labelof") + " {pages}",
                    displayMsg: pager.data("labeldisplayitems")
                });
                pager.removeClass("hide");
            }
            else {
                pager.addClass("hide");
            }
            return pager;
        };
        this.onChange = function () {
            _this.$reportForm.trigger("submit");
        };
        this.updateHiddenFields = function (pageNumber, pageSize) {
            _this.$pageIndex.val(pageNumber);
            _this.$pageSize.val(pageSize);
        };
        this.isShow = function () {
            // total records less than min page size => do not need to show pager
            return _this.pageList[0] <= _this.totalRecords;
        };
        this.options = new PagerOption(this.onChange);
        this.$ = $;
    }
    return PagerComponent;
}());
//# sourceMappingURL=pager.js.map
"use strict";
var SiteInfrastructure = (function () {
    function SiteInfrastructure() {
        var _this = this;
        this.pageList = [50, 100, 200, 500];
        this.customerStatus = {
            all: 0,
            open: 1,
            closed: 3,
            suspended: 2
        };
        this.closeReasonConstant = {
            LogInFailedOrExpired: 0,
            SecurityCode: 3,
            Upline: 2,
            Admin: 1
        };
        this.roleId = {
            master: 3,
            agent: 2,
            member: 1
        };
        this.localStorage = {
            get: function (key) {
                return store.enabled ? store.get(key) : $.cookie(key);
            },
            set: function (key, value) {
                store.enabled ? store.set(key, value) : $.cookie(key, value, _this.localStorage.getCookieExpireOptions());
            },
            check: function (key) {
                return typeof store.get(key) !== 'undefined' || typeof $.cookie(key) !== 'undefined';
            },
            getCookieExpireOptions: function () {
                var cookieExpire = { expires: 365, path: window.location.pathname };
                return cookieExpire;
            }
        };
    }
    return SiteInfrastructure;
}());
var siteInfrastructure = new SiteInfrastructure();
//# sourceMappingURL=site-infrastructure.js.map
var BaseList = (function () {
    function BaseList($, statusComponent, numeral, pager, levelName) {
        var _this = this;
        this.reportParam = {
            DownlineUsername: null,
            Username: "",
            SearchRoleId: 0,
            Status: 0,
            PageSize: 100,
            PageIndex: 1
        };
        this.indexManager = null;
        this.viewList = function () {
            _this.$.blockUI();
            _this.reportParam = {
                DownlineUsername: null,
                Username: _this.$username.val(),
                SearchRoleId: _this.levelId,
                Status: _this.$dropdownStatus.find("option:selected").val(),
                PageSize: _this.reportParam.PageSize,
                PageIndex: _this.reportParam.PageIndex
            };
            _this.loadData();
        };
        this.pagingList = function () {
            _this.reportParam.PageIndex = _this.$("#PageIndex" + _this.levelName).val();
            _this.reportParam.PageSize = _this.$("#PageSize" + _this.levelName).val();
            _this.loadData();
        };
        this.$ = $;
        this.statusComponent = statusComponent;
        this.numeral = numeral;
        this.pager = pager;
        this.levelName = levelName;
        this.levelNameLowerCase = levelName.toLowerCase();
    }
    BaseList.prototype.init = function (viewResources, indexManager, renderList, levelId, viewDownlineFunction, addDownlineFunction) {
        this.viewResources = viewResources;
        this.indexManager = indexManager;
        this.renderList = renderList;
        this.levelId = levelId;
        this.reportColSpan = levelId == siteInfrastructure.roleId.member ? 8 : 7;
        this.viewDownlineFunction = viewDownlineFunction;
        this.addDownlineFunction = addDownlineFunction;
        this.cacheDOMElements();
        this.statusComponent.render(new SingleSelectionDropdownOption("dropdown-status-" + this.levelNameLowerCase, "UserSelectedStatusId" + this.levelName));
        this.$dropdownStatus = this.$("#dropdown-status-" + this.levelNameLowerCase);
        this.$tblReport.stickyTableHeaders();
        var self = this;
        this.$("#btn-submit-" + this.levelName.toLowerCase()).click(function () {
            self.reportParam.PageSize = 100;
            self.reportParam.PageIndex = 1;
            self.viewList();
        });
    };
    BaseList.prototype.cacheDOMElements = function () {
        this.$username = this.$("#txt-username-" + this.levelNameLowerCase);
        this.getCustomerListUrl = this.$("#getCustomerListUrl").val();
        this.$tblReport = this.$("#tbl-" + this.levelNameLowerCase);
        this.HasUpdatePermission = this.$("#HasUpdatePermission").val() == "1";
        this.HasAddNewPermission = this.$("#HasAddNewPermission").val() == "1";
    };
    ;
    BaseList.prototype.loadData = function () {
        var _this = this;
        this.$.ajax({
            method: "GET",
            data: this.reportParam,
            url: this.getCustomerListUrl,
            cache: false,
            success: function (rs) {
                _this.renderList(rs);
            }
        });
    };
    BaseList.prototype.bindReportEvents = function (rs) {
        var self = this;
        this.$tblReport.find(".view-downline").click(function () {
            var $this = self.$(this);
            self.viewDownlineFunction($this.attr("cust-id"), $this.html());
        });
        this.$tblReport.find(".update-credit").click(function () {
            var $this = self.$(this);
            self.indexManager.updateCredit($this.attr("cust-id"), $this.attr("username"), self.levelId);
        });
        this.$tblReport.find(".update-status").click(function () {
            var $this = self.$(this);
            self.indexManager.updateStatus($this.attr("cust-id"), $this.attr("username"), $this.attr("current-status"));
        });
        if (this.HasUpdatePermission) {
            this.$tblReport.find(".icon-edit").click(function () {
                var $this = self.$(this);
                self.indexManager.updateAccount($this.attr("cust-id"), $this.attr("username"), self.levelId, self.reportParam);
            });
        }
        if (this.HasAddNewPermission) {
            this.$tblReport.find(".icon-add").click(function () {
                var $this = self.$(this);
                self.addDownlineFunction($this.attr("cust-id"), $this.attr("username"), self.levelId - 1, self.reportParam);
            });
        }
        this.$tblReport.find("> tfoot").empty();
        this.$("#PageIndex" + this.levelName).remove();
        this.$("#PageSize" + this.levelName).remove();
        if (rs.TotalCustomers >= siteInfrastructure.pageList[0]) {
            var footer = "<tr>" +
                "<td colspan='" + this.reportColSpan + "'><div id='report-pager-" + this.levelNameLowerCase + "' " +
                "data-labelpage='" + this.viewResources.Page + "'" +
                "data-labelof='" + this.viewResources.Of + "'" +
                "data-labeldisplayitems='" + this.viewResources.Pager_DisplayItems + "'" +
                "data-pageindex='" + this.reportParam.PageIndex + "'" +
                "data-pagesize='" + this.reportParam.PageSize + "'" +
                "data-totalrecords='" + rs.TotalCustomers + "'" +
                "></div></td>" +
                "</tr>";
            this.$tblReport.find("> tfoot").html(footer);
            this.pager.render(new PagerOption(this.pagingList, "report-pager-" + this.levelNameLowerCase, "form-" + this.levelNameLowerCase, "PageIndex" + this.levelName, "PageSize" + this.levelName));
        }
        this.$.unblockUI();
    };
    return BaseList;
}());
//# sourceMappingURL=base-list.js.map
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var MasterList = (function (_super) {
    __extends(MasterList, _super);
    function MasterList($, statusComponent, numeral, pager) {
        var _this = _super.call(this, $, statusComponent, numeral, pager, "Master") || this;
        _this.renderMasterList = function (rs) {
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
        _this.reloadState = function (reportParam) {
            _this.$username.val(reportParam.Username);
            _this.$dropdownStatus.val(reportParam.Status);
            _this.reportParam.PageSize = reportParam.PageSize;
            _this.reportParam.PageIndex = reportParam.PageIndex;
            _this.$("#PageIndexMaster").val(reportParam.PageIndex);
            _this.$("#PageSizeMaster").val(reportParam.PageIndex);
            _this.viewList();
        };
        return _this;
    }
    MasterList.prototype.init = function (viewResources, indexManager) {
        _super.prototype.init.call(this, viewResources, indexManager, this.renderMasterList, siteInfrastructure.roleId.master, indexManager.viewMaster, indexManager.addAgentForMaster);
    };
    return MasterList;
}(BaseList));
//# sourceMappingURL=master-list.js.map
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var AgentList = (function (_super) {
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
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var MemberList = (function (_super) {
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
var CustomerList = (function () {
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
            //let width = this.topWindow.displayTextWidth(popupTitle, this.$(".col-username").css("fontSize"));
            _this.topWindow.popupManager.open(url, popupTitle, 500, 250);
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