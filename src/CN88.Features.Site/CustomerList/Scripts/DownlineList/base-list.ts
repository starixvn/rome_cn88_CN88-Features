/// <reference path="../../../shared/scripts/components/pager.ts" />
/// <reference path="../../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="viewmodel/view-model.ts" />
/// <reference path="../../../shared/scripts/components/html-wrapper.ts" />

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
    isIndirectDownlineOfActor: boolean;
    directDownlineRoleId: number;

    minPageSize: number = 10;
    customerListHelper: CustomerListHelper;
    canDoAddOrUpdateAction: boolean;
    isShowEditSettingLink: boolean;

    constructor(
        $: JQueryStatic,
        topWindow: any,
        numeral: any,
        levelId: number,
        levelName: string,
        canDoAddOrUpdateAction: boolean,
        isShowEditSettingLink: boolean,
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
        this.canDoAddOrUpdateAction = canDoAddOrUpdateAction;
        this.isShowEditSettingLink = isShowEditSettingLink;
    }

    public init(indexManager: CustomerList, downlineObject: BaseList) {
        this.indexManager = indexManager;
        this.viewResources = indexManager.viewResources;

        this.isIndirectDownlineOfActor = this.levelId < this.indexManager.ActorLevel - 1;
        this.directDownlineRoleId = this.levelId - 1;
        this.cacheDOMElements();

        this.customerListHelper = new CustomerListHelper(indexManager.viewResources, indexManager.numeral);
        this.$downlineObject = downlineObject;
        this.statusComponent.render(
            new SingleSelectionDropdownOption(`dropdown-status-${this.levelNameLowerCase}`, `UserSelectedStatusId${this.levelName}`));

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
        this.$dropdownStatus = this.$(`#dropdown-status-${this.levelNameLowerCase}`);
        this.getCustomerListUrl = this.$("#getCustomerListUrl").val();
        this.$tblReport = this.$(`#tbl-${this.levelNameLowerCase}`);
        this.$formReport = this.$(`#form-${this.levelNameLowerCase}`);

        if (this.isIndirectDownlineOfActor) {
            this.$viewedDirectUpline = this.$(`#txt-viewed-${this.levelId === siteInfrastructure.roleId.agent ? 'master' : 'agent'}`);
            this.reportParam = new ReportParam();
        }
        else {
            this.reportParam = {
                DownlineUsername: this.$("#DownlineUsername").val(),
                Username: <string>this.$("#Username").val(),
                SearchRoleId: parseInt(<string>this.$("#SearchRoleId").val()),
                CustId: 0,
                Status: parseInt(<string>this.$("#Status").val()),
                PageSize: parseInt(<string>this.$("#PageSize").val()),
                PageIndex: 1
            };
        }
    };

    loadData = (isPagingClick = true) => {
        const downlineUsername = this.isIndirectDownlineOfActor ? this.$viewedDirectUpline.val() : null,
            pagerContent = this.$(`#report-pager-${this.levelNameLowerCase}`).html(),
            existedPager = pagerContent !== '' && pagerContent !== undefined;

        this.reportParam.DownlineUsername = downlineUsername;
        this.reportParam.Username = this.$username.val();
        this.reportParam.SearchRoleId = this.levelId;
        this.reportParam.Status = <number>this.$dropdownStatus.find("option:selected").val();

        if (isPagingClick && existedPager) {
            this.reportParam.PageIndex = this.$(`#PageIndex${this.levelName}`).val();
            this.reportParam.PageSize = this.$(`#PageSize${this.levelName}`).val();
        }

        localStorage.setItem(`${this.levelName}ReportParam`, JSON.stringify(this.reportParam));

        this.$.ajax({
            method: "GET",
            data: this.reportParam,
            url: this.getCustomerListUrl,
            cache: false,
            success: (rs) => {
                this.renderCustomerList(rs);
            },
            complete: this.$.unblockUI
        });
    }

    bindReportEvents() {
        this.$tblReport.find(".view-statement").click(event => this.indexManager.viewStatement(event));
        this.$tblReport.find(".view-downline").click(event => this.viewDownlines(event));
        this.$tblReport.find(".col-credit").click(event => this.indexManager.updateCredit(event, this.levelId));
        this.$tblReport.find(".col-status").click(event => this.indexManager.updateStatus(event, this.levelId));

        this.$tblReport.find(".icon-pass").click(event => this.indexManager.resetPassword(event));
        this.$tblReport.find(".icon-security").click(event => this.indexManager.resetSecurityCode(event));
        this.$tblReport.find(".icon-edit").click(event => this.indexManager.updateAccount(this.$(event.currentTarget).attr("cust-id"), this.levelId, this.reportParam));
        this.$tblReport.find(".icon-add").click(event => this.indexManager.addNewDownline(this.$(event.currentTarget).attr("cust-id"), this.directDownlineRoleId, this.reportParam));
        this.$tblReport.find(".icon-unlock").click(event => this.indexManager.updateUnlock(event));

        this.$('[data-toggle="tooltip"]').tooltip();
    };

    bindingAddNewDirectDownlineButton(rs) {
        const { SearchAccountId, SearchAccountStatus } = rs;
        const buttonAddNew = this.$(`.add-directdownline-${this.levelNameLowerCase}`);
        if (SearchAccountId > 0 && SearchAccountStatus === siteInfrastructure.customerStatus.active) {
            buttonAddNew.removeAttr("disabled");
            buttonAddNew.click(() => this.indexManager.addNewDownline(SearchAccountId, this.levelId, this.reportParam));
        }
        else {
            buttonAddNew.attr("disabled", "disabled");
        }
    }

    renderPaging(totalCustomers: number) {
        const pageIndex = `PageIndex${this.levelName}`, pageSize = `PageSize${this.levelName}`;
        this.$tblReport.find("> tfoot").empty();
        this.$(`#${pageIndex}`).remove();
        this.$(`#${pageSize}`).remove();

        const pagerElement = this.$(`#pager-${this.levelNameLowerCase}`);
        if (totalCustomers >= this.minPageSize && pagerElement.length > 0) {
            var footer = `<div id='report-pager-${this.levelNameLowerCase}'` +
                `data-labelpage='${this.viewResources.Page}'` +
                `data-labelof='${this.viewResources.Of}'` +
                `data-labeldisplayitems='${this.viewResources.Pager_DisplayItems}'` +
                `data-pageindex='${this.reportParam.PageIndex}'` +
                `data-pagesize='${this.reportParam.PageSize}'` +
                `data-totalrecords='${totalCustomers}'></div>`;

            pagerElement.html(footer);
            this.pager.render(new PagerOption(this.loadData, `report-pager-${this.levelNameLowerCase}`, `form-${this.levelNameLowerCase}`, pageIndex, pageSize));
        } else {
            pagerElement.html("");
        }
    }

    renderCustomerList(rs: any) {
        this.bindingAddNewDirectDownlineButton(rs);

        if (!rs.Customers) {
            return;
        }

        this.$tblReport.removeClass("display-none");

        this.$tblReport.find("> tbody").empty().html(
            this.customerListHelper.buildBody(rs, !this.indexManager.isDevice, this.canDoAddOrUpdateAction, this.levelId, this.reportParam, this.isShowEditSettingLink)
        );
        this.$(window).trigger('resize.stickyTableHeaders');

        this.bindReportEvents();
        this.renderPaging(rs.TotalCustomers);
    };

    reloadPage() {
        const reportParamLocalStorage = JSON.parse(localStorage.getItem(`${this.levelName}ReportParam`));
        if (reportParamLocalStorage !== null && reportParamLocalStorage !== undefined) {
            this.reportParam = JSON.parse(localStorage.getItem(`${this.levelName}ReportParam`));
        }

        const { Username, DownlineUsername } = this.reportParam;
        this.$username.val(Username);
        if (this.isIndirectDownlineOfActor) {
            this.$viewedDirectUpline.val(DownlineUsername);
        }

        this.viewList();
    };

    viewDownlines(event: any) {
        const $this = this.$(event.currentTarget),
            custId = parseInt($this.attr("cust-id")),
            username = this.indexManager.isDevice ? $this.attr("username") : $this.html();

        this.$(`#tab${this.directDownlineRoleId}`).click();

        this.$downlineObject.$viewedDirectUpline.val(username);
        this.$downlineObject.$username.val('');

        this.$downlineObject.reportParam.PageIndex = 1;
        this.$downlineObject.reportParam.PageSize = 100;
        this.$downlineObject.reportParam.CustId = custId;

        this.$downlineObject.statusComponent.changeSelection(this.reportParam.Status);
        this.$downlineObject.viewList();
    };

    viewList() {
        this.$.blockUI();

        if (this.indexManager.isDevice) {
            this.toggleHandler.hideFilter();
        }

        if (this.isIndirectDownlineOfActor) {
            const $err = this.$(`#msg-err-${this.levelNameLowerCase}`);
            const downlineUsername = this.isIndirectDownlineOfActor ? this.$viewedDirectUpline.val() : null;
            if (downlineUsername === '' || downlineUsername === undefined) {
                $err.removeClass("display-none");
                this.$.unblockUI();
                return;
            }

            $err.addClass("display-none");
        }

        this.loadData(false);
    }
}