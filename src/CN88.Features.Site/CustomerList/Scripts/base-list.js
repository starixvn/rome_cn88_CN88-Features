var BaseList = /** @class */ (function () {
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