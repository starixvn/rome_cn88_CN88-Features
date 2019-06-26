/// <reference path="../../shared/scripts/components/pager.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="viewmodel/view-model.ts" />
/// <reference path="../../shared/scripts/components/html-wrapper.ts" />

class BaseList {
    $: JQueryStatic;
    numeral: any;
    statusComponent: SingleSelectionDropdownComponent;
    pager: PagerComponent;
    bootstrapDropdownComponent: BootstrapDropdownComponent;

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

    viewResources: ViewResources;
    indexManager: any = null;

    levelName: string;
    levelNameLowerCase: string;

    levelId: number;
    viewDownlineFunction: Function;
    minPageSize: number = 10;
    viewerStatusId: number;
    customerListHelper: CustomerListHelper;

    constructor(
        $: JQueryStatic,
        statusComponent: SingleSelectionDropdownComponent,
        bootstrapDropdownComponent: BootstrapDropdownComponent,
        numeral: any,
        pager: PagerComponent,
        levelName: string) {
        this.$ = $;
        this.statusComponent = statusComponent;
        this.bootstrapDropdownComponent = bootstrapDropdownComponent;
        this.numeral = numeral;
        this.pager = pager;
        this.levelName = levelName;
        this.levelNameLowerCase = levelName.toLowerCase();
    }

    public init(
        indexManager: CustomerList,
        levelId: number,
        viewDownlineFunction: Function) {
        this.indexManager = indexManager;
        this.viewResources = indexManager.viewResources;

        this.customerListHelper = new CustomerListHelper(indexManager.viewResources, indexManager.getStatusName, indexManager.numeral);

        this.levelId = levelId;
        this.viewDownlineFunction = viewDownlineFunction;
        this.viewerStatusId = parseInt(this.$("#ViewerStatusId").val());
        this.cacheDOMElements();

        this.statusComponent.render(
            new SingleSelectionDropdownOption(`dropdown-status-${this.levelNameLowerCase}`, `UserSelectedStatusId${this.levelName}`));
        this.$dropdownStatus = this.$(`#dropdown-status-${this.levelNameLowerCase}`);
        this.$tblReport.stickyTableHeaders();
    }

    public cacheDOMElements() {
        this.$username = this.$(`#txt-username-${this.levelNameLowerCase}`);
        this.getCustomerListUrl = this.$("#getCustomerListUrl").val();
        this.$tblReport = this.$(`#tbl-${this.levelNameLowerCase}`);
        this.$formReport = this.$(`#form-${this.levelNameLowerCase}`);
    };

    loadData() {
        this.$.ajax({
            method: "GET",
            data: this.reportParam,
            url: this.getCustomerListUrl,
            cache: false,
            success: (rs) => {
                this.renderCustomerList(rs);
            }
        });
    }

    initTooltip = () => {
        this.$('[data-toggle="tooltip"]').tooltip();
    };

    pagingList = () => {
        this.reportParam.PageIndex = this.$(`#PageIndex${this.levelName}`).val();
        this.reportParam.PageSize = this.$(`#PageSize${this.levelName}`).val();
        this.loadData();
    };

    bindReportEvents(rs) {
        var self = this;
        this.$tblReport.find(".view-statement").click(function () {
            var $this = self.$(this);
            var viewStatementUrl = `${$("#viewStatementtUrl").val()}?CustName=${$this.attr("username")}&CustId=${$this.attr("cust-id")}`;
            self.indexManager.isDevice ?
                window.open(viewStatementUrl, "_blank") :
                window.open(viewStatementUrl, "MemberStatement", "scrollbars=1,height=500,width=1000");
        });

        this.$tblReport.find(".view-downline").click(function () {
            var $this = self.$(this);
            self.indexManager.isDevice ?
                self.viewDownlineFunction($this.attr("cust-id"), $this.attr("username"), self.reportParam.Status) :
                self.viewDownlineFunction($this.attr("cust-id"), $this.html(), self.reportParam.Status);
        });

        this.$tblReport.find(".col-credit").click(function () {
            var $this = self.$(this);

            self.indexManager.updateCredit($this.attr("cust-id"), $this.attr("username"), self.levelId);
        });

        this.$tblReport.find(".col-status").click(function () {
            var $this = self.$(this);

            self.indexManager.updateStatus($this.attr("cust-id"), $this.attr("username"), self.levelId, $this.attr("current-status"), $this.attr("direct-upline-status"));
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
            self.indexManager.addNewDownline($this.attr("cust-id"), '', self.levelId - 1, self.reportParam, $this.attr("username"));
        });

        var buttonAddNew = this.$(`.add-directdownline-${this.levelNameLowerCase}`);
        this.$tblReport.find(".icon-unlock").click(function () {
            var $this = self.$(this);

            self.indexManager.updateUnlock($this.attr("cust-id"), $this.attr("username"));
        });

        if (rs.SearchAccountId > 0 && parseInt(rs.SearchAccountStatus) === siteInfrastructure.customerStatus.active) {
            buttonAddNew.removeAttr("disabled");
            buttonAddNew.click(function () {
                var $this = self.$(this);

                self.indexManager.addNewDownline($this.attr("cust-id"), $this.attr("username"), self.levelId, self.reportParam, $this.attr("downline-username"));
            });
        }
        else {
            buttonAddNew.attr("disabled", "disabled");
        }

        const pageIndex = `PageIndex${this.levelName}`, pageSize = `PageSize${this.levelName}`;
        this.$tblReport.find("> tfoot").empty();
        this.$(`#${pageIndex}`).remove();
        this.$(`#${pageSize}`).remove();

        const pagerElement = this.$(`#pager-${this.levelNameLowerCase}`);
        if (rs.TotalCustomers >= this.minPageSize && pagerElement.length > 0) {
            var footer = `<div id='report-pager-${this.levelNameLowerCase}'` +
                `data-labelpage='${this.viewResources.Page}'` +
                `data-labelof='${this.viewResources.Of}'` +
                `data-labeldisplayitems='${this.viewResources.Pager_DisplayItems}'` +
                `data-pageindex='${this.reportParam.PageIndex}'` +
                `data-pagesize='${this.reportParam.PageSize}'` +
                `data-totalrecords='${rs.TotalCustomers}'></div>`;

            pagerElement.html(footer);
            this.pager.render(new PagerOption(this.pagingList, `report-pager-${this.levelNameLowerCase}`, `form-${this.levelNameLowerCase}`, pageIndex, pageSize));
        } else {
            pagerElement.html("");
        }

        this.bootstrapDropdownComponent.render(new BootstrapDropdownOption(".tbl-downlinelist .dropdown"));

        this.initTooltip();
        this.$.unblockUI();
    };

    renderCustomerList(rs: any) {
        if (rs.SearchAccountId > 0) {
            var buttonAddNew = this.$(`.add-directdownline-${this.levelNameLowerCase}`);
            buttonAddNew.attr("cust-id", rs.SearchAccountId);
            buttonAddNew.attr("username", rs.Username);
            buttonAddNew.attr("downline-username", rs.DownlineUsername);
        }

        if (!rs.Customers) {
            return;
        }

        this.$tblReport.removeClass("display-none");

        this.$tblReport.find("> tbody").empty().html(
            this.customerListHelper.buildBody(rs, !this.indexManager.isDevice, this.viewerStatusId, this.levelId, this.reportParam)
        );
        this.$(window).trigger('resize.stickyTableHeaders');
        this.bindReportEvents(rs);
    };

    baseReloadState(reportParam: any) {
        const { Username, PageSize, PageIndex } = reportParam;
        this.$username.val(Username);
        this.$(`#PageIndex${this.levelName}`).val(PageIndex);
        this.$(`#PageSize${this.levelName}`).val(PageSize);
    };

    setStateWhenViewDownlines(status: number) {
        this.$username.val("");
        this.reportParam.PageIndex = 1;
        this.reportParam.PageSize = 100;
        this.reportParam.Status = status;
        this.statusComponent.changeSelection(status);
    };

    viewList(downlineUsername: string = null, custId: number = 0) {
        this.$.blockUI();
        this.reportParam = {
            DownlineUsername: downlineUsername,
            Username: <string>this.$username.val(),
            SearchRoleId: this.levelId,
            CustId: custId,
            Status: <number>this.$dropdownStatus.find("option:selected").val(),
            PageSize: this.reportParam.PageSize,
            PageIndex: this.reportParam.PageIndex
        };

        const { masterToggleHandler, agentToggleHandler, memberToggleHandler } = this.indexManager,
            { master, agent } = siteInfrastructure.roleId;

        if (this.indexManager.isDevice) {
            this.levelId === master ?
                masterToggleHandler.hideFilter() :
                this.levelId === agent ? agentToggleHandler.hideFilter() :
                    memberToggleHandler.hideFilter();
        }

        this.loadData();
    }
}
