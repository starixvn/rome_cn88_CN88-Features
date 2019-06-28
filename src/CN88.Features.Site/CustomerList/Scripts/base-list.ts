/// <reference path="../../shared/scripts/components/pager.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="viewmodel/view-model.ts" />
/// <reference path="../../shared/scripts/components/html-wrapper.ts" />

class BaseList {
    $: JQueryStatic;
    topWindow: any;
    numeral: any;
    statusComponent: SingleSelectionDropdownComponent;
    toggleHandler: ToggleHandler;
    pager: PagerComponent;
    bootstrapDropdownComponent: BootstrapDropdownComponent;

    $tooltip: JQuery;
    $dropdownStatus: JQuery;
    $username: JQuery;
    $tblReport: JQuery;
    $formReport: JQuery;
    getCustomerListUrl: string;

    $viewedDirectUpline: JQuery;

    $downlineObject: BaseList;
    reportParam: ReportParam;

    viewResources: ViewResources;
    indexManager: CustomerList;

    levelName: string;
    levelNameLowerCase: string;
    levelId: number;

    minPageSize: number = 10;
    viewerStatusId: number;
    customerListHelper: CustomerListHelper;

    constructor(
        $: JQueryStatic,
        topWindow: any,
        numeral: any,
        levelId: number,
        levelName: string,
        toggleHandler: ToggleHandler = null,
        pager: PagerComponent = null,
        bootstrapDropdownComponent: BootstrapDropdownComponent = null,
        statusComponent: SingleSelectionDropdownComponent = null
    ) {
        this.$ = $;
        this.topWindow = topWindow;
        this.statusComponent = statusComponent === null ? new SingleSelectionDropdownComponent($) : statusComponent;
        this.bootstrapDropdownComponent = bootstrapDropdownComponent === null ? new BootstrapDropdownComponent($, topWindow) : bootstrapDropdownComponent;
        this.numeral = numeral;
        this.pager = pager === null ? new PagerComponent($) : pager;
        this.toggleHandler = toggleHandler === null ? new ToggleHandler($) : toggleHandler;
        this.levelName = levelName;
        this.levelNameLowerCase = levelName.toLowerCase();
        this.levelId = levelId;
    }

    public init(indexManager: CustomerList, downlineObject: BaseList) {
        this.cacheDOMElements();
        this.indexManager = indexManager;
        this.viewResources = indexManager.viewResources;
        this.reportParam = indexManager.reportParam;

        this.customerListHelper = new CustomerListHelper(indexManager.viewResources, indexManager.numeral);
        this.$downlineObject = downlineObject;
        this.statusComponent.render(
            new SingleSelectionDropdownOption(`dropdown-status-${this.levelNameLowerCase}`, `UserSelectedStatusId${this.levelName}`));

        this.$dropdownStatus = this.$(`#dropdown-status-${this.levelNameLowerCase}`);
        this.$tblReport.stickyTableHeaders();

        if (this.indexManager.isDevice) {
            this.toggleHandler.render(new ToggleHandlerOption(`${this.levelNameLowerCase}-filter-toggle-icon`, `form-${this.levelNameLowerCase}`, `${this.levelNameLowerCase}-container`));
        }
        this.$(`#btn-submit-${this.levelNameLowerCase}`).off("click").on("click", () => {
            this.reportParam.PageIndex = 1;
            this.viewList();
        });
    }

    public cacheDOMElements() {
        this.$username = this.$(`#txt-username-${this.levelNameLowerCase}`);
        this.getCustomerListUrl = this.$("#getCustomerListUrl").val();
        this.$tblReport = this.$(`#tbl-${this.levelNameLowerCase}`);
        this.$formReport = this.$(`#form-${this.levelNameLowerCase}`);
        this.viewerStatusId = parseInt(this.$("#ViewerStatusId").val());

        if (this.levelId < siteInfrastructure.roleId.master) {
            this.$viewedDirectUpline = this.$(`#txt-viewed-${this.levelId === siteInfrastructure.roleId.agent ? 'master' : 'agent'}`);
        }
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

    initTooltip() {
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

        this.$tblReport.find(".view-downline").click(event => this.viewDownlines(event));
        this.$tblReport.find(".col-credit").click(event => this.indexManager.updateCredit(event, this.levelId));
        this.$tblReport.find(".col-status").click(event => this.indexManager.updateStatus(event, this.levelId));
        this.$tblReport.find(".icon-pass").click(event => this.indexManager.resetPassword(event));
        this.$tblReport.find(".icon-security").click(event => this.indexManager.resetSecurityCode(event));

        this.$tblReport.find(".icon-edit").click(event =>
            this.indexManager.updateAccount(this.$(event.currentTarget).attr("cust-id"), this.levelId, this.reportParam));

        this.$tblReport.find(".icon-add").click(event => this.indexManager.addNewDownline(event, this.levelId - 1, this.reportParam));
        this.$tblReport.find(".icon-unlock").click(event => this.indexManager.updateUnlock(event));

        const buttonAddNew = this.$(`.add-directdownline-${this.levelNameLowerCase}`);
        if (rs.SearchAccountId > 0 && parseInt(rs.SearchAccountStatus) === siteInfrastructure.customerStatus.active) {
            buttonAddNew.removeAttr("disabled");
            buttonAddNew.click(event => this.indexManager.addNewDownline(event, this.levelId, this.reportParam, this.$(event.currentTarget).attr("downline-username")));
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

    reloadState(reportParam: ReportParam) {
        const { Username, PageSize, PageIndex, DownlineUsername } = reportParam;
        if (this.levelId < this.indexManager.ActorLevel - 1) {
            this.$viewedDirectUpline.val(DownlineUsername);
        }

        this.$username.val(Username);
        this.$(`#PageIndex${this.levelName}`).val(PageIndex);
        this.$(`#PageSize${this.levelName}`).val(PageSize);

        this.viewList();
    };

    viewDownlines = (event: any) => {
        const $this = this.$(event.currentTarget),
            custId = parseInt($this.attr("cust-id")),
            username = this.indexManager.isDevice ? $this.attr("username") : $this.html();

        this.$(`.lnk-changetab[level='${this.levelId - 1}']`).click();

        this.$downlineObject.$viewedDirectUpline.val(username);
        this.$downlineObject.$username.val("");

        this.reportParam.PageIndex = 1;
        this.reportParam.PageSize = 100;
        this.reportParam.CustId = custId;
        this.reportParam.Username = username;

        this.statusComponent.changeSelection(this.reportParam.Status);
        this.$downlineObject.viewList();
    };

    viewList() {
        this.$.blockUI();
        this.reportParam.DownlineUsername =
            this.levelId === this.indexManager.ActorLevel - 1 ? null : this.$viewedDirectUpline.val();

        this.reportParam.Username = this.$username.val();
        this.reportParam.SearchRoleId = this.levelId;
        this.reportParam.Status = <number>this.$dropdownStatus.find("option:selected").val();

        if (this.levelId < this.indexManager.ActorLevel - 1) {
            const directDownline = parseInt(<string>this.$("#directDownline").val()),
                $err = this.$(`#msg-err-${this.levelNameLowerCase}`),
                { DownlineUsername } = this.reportParam;

            if (directDownline != this.levelId &&
                (DownlineUsername == "" || DownlineUsername == undefined)) {
                $err.removeClass("display-none");
                this.$.unblockUI();
                return;
            }

            $err.addClass("display-none");
        }

        if (this.indexManager.isDevice) {
            this.toggleHandler.hideFilter();
        }

        this.loadData();
    }
}
