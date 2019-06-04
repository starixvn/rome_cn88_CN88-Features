/// <reference path="../../shared/scripts/components/pager.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />

class BaseList {
    $: JQueryStatic;
    numeral: any;
    statusComponent: SingleSelectionDropdownComponent;
    pager: PagerComponent;

    $tooltip: JQuery;
    $dropdownStatus: JQuery;
    $username: JQuery;
    $tblReport: JQuery;
    $formReport: JQuery;
    getCustomerListUrl: string;

    reportParam = {
        DownlineUsername: null,
        Username: "",
        SearchRoleId: 0,
        CustId: 0,
        Status: 0,
        PageSize: 100,
        PageIndex: 1
    };

    viewResources: any;
    indexManager: any = null;

    levelName: string;
    levelNameLowerCase: string;
    renderList: Function;

    levelId: number;
    viewDownlineFunction: Function;
    addDownlineFunction: Function;
    reportColSpan: number;
    minPageSize: number = 10;

    constructor(
        $: JQueryStatic,
        statusComponent: SingleSelectionDropdownComponent,
        numeral: any,
        pager: PagerComponent,
        levelName: string) {
        this.$ = $;
        this.statusComponent = statusComponent;
        this.numeral = numeral;
        this.pager = pager;
        this.levelName = levelName;
        this.levelNameLowerCase = levelName.toLowerCase();
    }

    public init(
        viewResources: any,
        indexManager: any,
        renderList: Function,
        levelId: number,
        viewDownlineFunction: Function,
        addDownlineFunction: Function) {
        this.viewResources = viewResources;
        this.indexManager = indexManager;
        this.renderList = renderList;
        this.levelId = levelId;
        this.reportColSpan = levelId == siteInfrastructure.roleId.member ? 9 : 8;
        this.viewDownlineFunction = viewDownlineFunction;
        this.addDownlineFunction = addDownlineFunction;

        this.cacheDOMElements();

        this.statusComponent.render(
            new SingleSelectionDropdownOption("dropdown-status-" + this.levelNameLowerCase, "UserSelectedStatusId" + this.levelName));
        this.$dropdownStatus = this.$("#dropdown-status-" + this.levelNameLowerCase);
        this.$tblReport.stickyTableHeaders();
        var self = this;
        this.$("#btn-submit-" + this.levelName.toLowerCase()).click(function () {
            self.reportParam.PageIndex = 1;

            if (self.indexManager.isDevice) {
                self.indexManager.masterToggleHandler.hideFilter();
            }

            self.viewList();
        });
    }

    public cacheDOMElements() {
        this.$username = this.$("#txt-username-" + this.levelNameLowerCase);
        this.getCustomerListUrl = this.$("#getCustomerListUrl").val();
        this.$tblReport = this.$("#tbl-" + this.levelNameLowerCase);
        this.$formReport = this.$("#form-" + this.levelNameLowerCase);
    };
    public loadData() {
        this.$.ajax({
            method: "GET",
            data: this.reportParam,
            url: this.getCustomerListUrl,
            cache: false,
            success: (rs) => {
                this.renderList(rs);
            }
        });
    }

    initTooltip = () => {
        this.$('[data-toggle="tooltip"]').tooltip();
    };

    viewList = () => {
        this.$.blockUI();
        this.reportParam = {
            DownlineUsername: null,
            Username: <string>this.$username.val(),
            SearchRoleId: this.levelId,
            CustId: 0,
            Status: <number>this.$dropdownStatus.find("option:selected").val(),
            PageSize: this.reportParam.PageSize,
            PageIndex: this.reportParam.PageIndex
        };
        this.loadData();
    };
    pagingList = () => {
        this.reportParam.PageIndex = <number>this.$("#PageIndex" + this.levelName).val();
        this.reportParam.PageSize = <number>this.$("#PageSize" + this.levelName).val();
        this.loadData();
    };
    public bindReportEvents(rs) {
        var self = this;
        this.$tblReport.find(".view-statement").click(function () {
            var $this = self.$(this);
            var viewStatementUrl = $("#viewStatementtUrl").val() + "?CustName=" + $this.attr("username") + "&CustId=" + $this.attr("cust-id");

            if (self.indexManager.isDevice) {
                window.open(viewStatementUrl, "_blank");
            } else {
                window.open(viewStatementUrl, "MemberStatement", "scrollbars=1,height=500,width=1000");
            }
        });

        this.$tblReport.find(".view-downline").click(function () {
            var $this = self.$(this);
            if (self.indexManager.isDevice) {
                self.viewDownlineFunction($this.attr("cust-id"), $this.attr("username"));
            } else {
                self.viewDownlineFunction($this.attr("cust-id"), $this.html());
            }
        });

        this.$tblReport.find(".col-credit").click(function () {
            var $this = self.$(this);

            self.indexManager.updateCredit($this.attr("cust-id"), $this.attr("username"), self.levelId);
        });

        this.$tblReport.find(".col-status").click(function () {
            var $this = self.$(this);

            self.indexManager.updateStatus($this.attr("cust-id"), $this.attr("username"), $this.attr("current-status"));
        });

        this.$tblReport.find(".icon-pass").click(function () {
            var $this = self.$(this);

            self.indexManager.resetPassword($this.attr("cust-id"), $this.attr("username"), $this.attr("cust-role"));
        });

        this.$tblReport.find(".icon-security").click(function () {
            var $this = self.$(this);

            self.indexManager.resetSecurityCode($this.attr("cust-id"), $this.attr("username"));
        });

        this.$tblReport.find(".icon-edit").click(function () {
            var $this = self.$(this);

            self.indexManager.updateAccount($this.attr("cust-id"), $this.attr("username"), self.levelId, self.reportParam);
        });

        this.$tblReport.find(".icon-add").click(function () {
            var $this = self.$(this);

            self.addDownlineFunction($this.attr("cust-id"), '', self.levelId - 1, self.reportParam, $this.attr("username"));
        });

        var buttonAddNew = this.$(".add-directdownline-" + this.levelNameLowerCase);
        if (rs.SearchAccountId > 0) {
            buttonAddNew.removeAttr("disabled");
            buttonAddNew.click(function () {
                var $this = self.$(this);

                self.addDownlineFunction($this.attr("cust-id"), $this.attr("username"), self.levelId, self.reportParam, $this.attr("downline-username"));
            });
        }
        else {
            buttonAddNew.attr("disabled", "disabled");
        }

        this.$tblReport.find("> tfoot").empty();
        this.$("#PageIndex" + this.levelName).remove();
        this.$("#PageSize" + this.levelName).remove();
        if (rs.TotalCustomers >= this.minPageSize) {
            var footer = "<div id='report-pager-" + this.levelNameLowerCase + "' " +
                "data-labelpage='" + this.viewResources.Page + "'" +
                "data-labelof='" + this.viewResources.Of + "'" +
                "data-labeldisplayitems='" + this.viewResources.Pager_DisplayItems + "'" +
                "data-pageindex='" + this.reportParam.PageIndex + "'" +
                "data-pagesize='" + this.reportParam.PageSize + "'" +
                "data-totalrecords='" + rs.TotalCustomers + "'" +
                "></div>";

            this.$("#pager-" + this.levelNameLowerCase).html(footer);
            this.pager.render(new PagerOption(this.pagingList, "report-pager-" + this.levelNameLowerCase, "form-" + this.levelNameLowerCase, "PageIndex" + this.levelName, "PageSize" + this.levelName));
        } else {
            this.$("#pager-" + this.levelNameLowerCase).html("");
        }

        this.initTooltip();
        this.$.unblockUI();
    }
}
