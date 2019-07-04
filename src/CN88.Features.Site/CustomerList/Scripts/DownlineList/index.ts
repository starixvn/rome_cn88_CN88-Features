/// <reference path="../../../shared/scripts/components/filter-toggle.ts" />
/// <reference path="../../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="../../../shared/scripts/components/pager.ts" />
/// <reference path="viewmodel/view-model.ts" />

class CustomerList {
    $: JQueryStatic;
    topWindow: Window;
    numeral: Numeral;
    securityCodeChecker: SecurityCodeChecker;

    $navTabs: JQuery;
    AccessDenyPage: string;
    isDevice = false;
    viewResources: ViewResources;

    masterList: BaseList;
    agentList: BaseList;
    memberList: BaseList;
    AccountManagementEditView: boolean;
    Language: string;
    ViewerId: number;
    ActorLevel: number;
    canDoAddOrUpdateAction: boolean;
    isShowEditSettingLink: boolean;

    constructor(
        $: JQueryStatic,
        numeral: Numeral,
        topWindow: Window,
        viewResources: ViewResources = null,
        securityCodeChecker: SecurityCodeChecker = null) {
        this.$ = $;
        this.topWindow = topWindow;
        this.numeral = numeral;
        this.viewResources = viewResources === null ? new ViewResources($) : viewResources;
        this.securityCodeChecker = securityCodeChecker === null ? new SecurityCodeChecker($, top) : securityCodeChecker;
    }

    init() {
        this.cacheDOMElements();
        this.topWindow["popupManager"].init(this.$, this.topWindow);
        this.initDownlineLists();

        this.$(".form-inline").bind("keypress", function (e) {
            if (e.keyCode === cn88Features.keyCode.enter) {
                return false;
            }
        });

        const searchAccountRoleId = parseInt(this.$("#SearchRoleId").val());
        if (searchAccountRoleId > 0) {
            this.$(`#tab${searchAccountRoleId}`).click();
            switch (searchAccountRoleId) {
                case siteInfrastructure.roleId.master:
                    this.masterList.reloadPage();
                    break;
                case siteInfrastructure.roleId.agent:
                    this.masterList && this.masterList.reloadPage();
                    this.agentList.reloadPage();
                    break;
                case siteInfrastructure.roleId.member:
                    this.masterList && this.masterList.reloadPage();
                    this.agentList && this.agentList.reloadPage();
                    this.memberList.reloadPage();
                    break;
            }
        } else {
            const directDownline = parseInt(this.$("#directDownline").val());
            switch (directDownline) {
                case siteInfrastructure.roleId.master:
                    this.masterList.viewList();
                    break;
                case siteInfrastructure.roleId.agent:
                    this.agentList.viewList();
                    break;
                case siteInfrastructure.roleId.member:
                    this.memberList.viewList();
                    break;
            }
        }
    }

    cacheDOMElements = () => {
        this.$navTabs = this.$("#nav-tabs");

        this.AccessDenyPage = this.$("#AccessDenyPage").val();
        this.AccountManagementEditView = this.$("#AccountManagementEditView").val() === "1";
        this.Language = this.$("#Language").val();
        this.ViewerId = parseInt(this.$("#ViewerId").val());
        this.ActorLevel = parseInt(this.$("#ActorLevel").val());
        this.isDevice = this.$("#IsDevice").val() === "1";

        this.canDoAddOrUpdateAction = this.$("#CanDoAddOrUpdateAction").val() === "1";
        this.isShowEditSettingLink = this.$("#IsShowEditSettingLink").val() === "1";
    };

    initDownlineLists(): void {
        this.memberList = new BaseList(this.$, this.topWindow, numeral, siteInfrastructure.roleId.member, "Member", this.canDoAddOrUpdateAction, this.isShowEditSettingLink);
        this.memberList.init(this, null);

        if (this.ActorLevel >= siteInfrastructure.roleId.master) {
            this.agentList = new BaseList(this.$, this.topWindow, numeral, siteInfrastructure.roleId.agent, "Agent", this.canDoAddOrUpdateAction, this.isShowEditSettingLink);
            this.agentList.init(this, this.memberList);
        }

        if (this.ActorLevel === siteInfrastructure.roleId.super) {
            this.masterList = new BaseList(this.$, this.topWindow, numeral, siteInfrastructure.roleId.master, "Master", this.canDoAddOrUpdateAction, this.isShowEditSettingLink);
            this.masterList.init(this, this.agentList);
        }
    }

    viewStatement(event) {
        const $this = this.$(event.currentTarget),
            viewStatementUrl = `${$('#viewStatementtUrl').val()}?CustName=${$this.attr('username')}&CustId=${$this.attr('cust-id')}`;

        this.isDevice ?
            window.open(viewStatementUrl, "_blank") :
            window.open(viewStatementUrl, "MemberStatement", "scrollbars=1,height=500,width=1000");
    }

    updateCredit(event, custLevelId: number) {
        const $this = this.$(event.currentTarget),
            updateCreditLink = `${this.$("#UpdateCreditLink").val()}?${this.$.param({ custId: $this.attr("cust-id"), custLevelId: custLevelId })}`,
            popupTitle = `${this.$('#UpdateCredit').val()} - ${$this.attr("username")}`;
        const popupWidth = 490, popupHeight = 280;
        this.openPopup(popupTitle, updateCreditLink, 'modal modal-credit', popupWidth, popupHeight);
    }

    updateStatus(event, custLevelId: number) {
        const $this = this.$(event.currentTarget),
            params = {
                custId: $this.attr("cust-id"),
                statusId: $this.attr("current-status"),
                custLevelId: custLevelId,
                uplineStatusId: $this.attr("direct-upline-status")
            },
            updateStatusLink = `${this.$('#UpdateStatusLink').val()}?${this.$.param(params)}`,
            popupTitle = `${this.$('#UpdateStatus').val()} - ${$this.attr("username")}`;

        const popupWidth = 490, popupHeight = 210;
        this.openPopup(popupTitle, updateStatusLink, 'modal modal-status', popupWidth, popupHeight);
    }

    resetPassword(event) {
        const $this = this.$(event.currentTarget), username = $this.attr("username"),
            params = { custId: $this.attr("cust-id"), username, custLevelId: $this.attr("cust-role") },
            resetPasswordLink = `${this.$('#ResetPasswordLink').val()}?${this.$.param(params)}`,
            popupTitle = `${this.$('#ResetPassword').val()} - ${username}`;

        const popupWidth = 490, popupHeight = 430;
        this.openPopup(popupTitle, resetPasswordLink, 'modal modal-password', popupWidth, popupHeight);
    }

    resetSecurityCode(event) {
        const $this = this.$(event.currentTarget),
            resetSecurityCodeLink = `${this.$("#ResetSecurityCodeLink").val()}?custId=${$this.attr("cust-id")}`,
            popupTitle = `${this.$('#ResetSecurityCode').val()} - ${$this.attr("username")}`;

        const popupWidth = 490, popupHeight = 344;
        this.openPopup(popupTitle, resetSecurityCodeLink, 'modal modal-password', popupWidth, popupHeight);
    }

    openPopup(title: string, url: string, popupCss: string, width = 480, height = 300) {
        this.topWindow["popupManager"].open({ title, url, width, height, popupCss });
    }

    updateAccount(custId: string, custLevelId: number, reportParam: ReportParam) {
        this.storeCurrentPage(reportParam);
        this.topWindow["showCN88CancelButton"] = true;
        this.topWindow["showCN88SaveAndNewButton"] = false;

        const editCustomerLink = this.$("#EditCustomerLink").val();
        window.location.href = `${editCustomerLink}?${this.$.param({
            Language: this.Language,
            ProductId: 1,
            CustomerLevel: custLevelId,
            CustomerId: custId,
            IsReadOnly: !this.AccountManagementEditView,
            UplineId: this.ViewerId,
            ActorLevel: this.ActorLevel
        })}`;
    }

    updateUnlock(event) {
        const $this = this.$(event.currentTarget),
            username = $this.attr("username"),
            custId = parseInt($this.attr("cust-id")),
            bootboxWidth = 350;

        this.topWindow["popupManager"].init(this.$, this.topWindow);
        this.topWindow["popupManager"].bootboxConfirm(
            `${this.viewResources.UnlockConfirmTitle} - ${username}`,
            this.viewResources.UnlockConfirmMessage,
            () => {
                this.$.ajax({
                    type: "POST",
                    url: 'UnlockCustomer',
                    data: { "CustId": custId },
                    dataType: "json",
                    headers: this.securityCodeChecker.setupAntiXSRF(),
                    success: (result) => {
                        this.securityCodeChecker.checkSecurityCode(result, {
                            url: this.$("#UnlockCustomerPath").val(),
                            data: { "CustId": custId },
                            onComplete: this.completedUnlock,
                            alertTitle: this.viewResources.UnlockConfirmTitle,
                            onCompleteParams: { custId: custId, $this: this }
                        });
                    },
                    complete: this.$.unblockUI
                });
            },
            bootboxWidth, this.viewResources.Ok, this.viewResources.Cancel
        );
    }

    completedUnlock(params) {
        const { custId, $this } = params;
        $this.isDevice ?
            $this.$(`tr[cust-id='${custId}']`).find('.icon-user-unlock').removeClass().addClass('icon-more-action') :
            $this.$(`.icon-unlock[cust-id='${custId}']`).remove();
    }

    addNewDownline = (custId: string, custLevelId, reportParam: ReportParam) => {
        const mainiFrame: JQuery = this.topWindow.$("#main");

        if (this.AccountManagementEditView) {
            this.storeCurrentPage(reportParam);
            this.topWindow["showCN88CancelButton"] = true;
            this.topWindow["showCN88SaveAndNewButton"] = true;

            const createCustomerLink = this.$("#CreateCustomerLink").val();
            mainiFrame.show();
            mainiFrame.attr("src", `${createCustomerLink}?${this.$.param({
                Language: this.Language,
                ProductId: 1,
                CustomerLevel: custLevelId,
                CustomerId: custId,
                IsReadOnly: !this.AccountManagementEditView,
                UplineId: custId,
                ActorLevel: this.ActorLevel
            })}`);
        } else {
            mainiFrame.attr("src", this.AccessDenyPage);
        }
    };

    storeCurrentPage(reportParam) {
        const currentPage = `${this.$("#CurrentLink").val()}?${this.$.param(reportParam)}`;
        cn88Features.localStorage.set("reloadCN88CustomerListCallBackUrl", { currentPage: currentPage });
    }

    reloadData = (levelName: string) => {
        const timeoutDefault = 2000;

        setTimeout(() => {
            const submitBtn = this.$(`input[id^='btn-submit-${levelName}']`)[0];
            if (submitBtn !== undefined) {
                submitBtn.click();
            }
        }, timeoutDefault);
    };
}

const customerList = new CustomerList($, numeral, window.top);

$(() => {
    customerList.init();
});