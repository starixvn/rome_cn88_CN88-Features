/// <reference path="../../shared/scripts/components/filter-toggle.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="../../shared/scripts/components/pager.ts" />
/// <reference path="viewmodel/view-model.ts" />

class CustomerList {
    $: JQueryStatic;
    topWindow: any;
    numeral: any;
    securityCodeChecker: SecurityCodeChecker;

    $navTabs: JQuery;
    AccessDenyPage: string;
    isDevice: boolean = false;

    reportParam: ReportParam;
    viewResources: ViewResources;

    masterList: BaseList;
    agentList: BaseList;
    memberList: BaseList;
    AccountManagementEditView: boolean;
    Language: string;
    ViewerId: number;
    ActorLevel: number;

    constructor(
        $: JQueryStatic,
        numeral: any,
        topWindow: any,
        viewResources: ViewResources = null,
        securityCodeChecker: SecurityCodeChecker = null) {
        this.$ = $;
        this.topWindow = topWindow;
        this.numeral = numeral;
        this.viewResources = viewResources === null ? new ViewResources($) : viewResources;
        this.securityCodeChecker = securityCodeChecker === null ? new SecurityCodeChecker($, top) : securityCodeChecker;

        this.masterList = new BaseList($, topWindow, numeral, siteInfrastructure.roleId.master, "Master");
        this.agentList = new BaseList($, topWindow, numeral, siteInfrastructure.roleId.agent, "Agent");
        this.memberList = new BaseList($, topWindow, numeral, siteInfrastructure.roleId.member, "Member");
    }

    init = () => {
        this.cacheDOMElements();

        this.masterList.init(this, this.agentList);
        this.agentList.init(this, this.memberList);
        this.memberList.init(this, null);

        const directDownline = this.$("#directDownline").val();
        this.$navTabs.find(`li[level='${directDownline}']`).addClass("active");
        this.$(`.tab-content[level='${directDownline}']`).removeClass("display-none");

        let self = this;
        this.$(".lnk-changetab").click(function () {
            let $this = self.$(this);
            self.$navTabs.find("> li").removeClass("active");
            $this.addClass("active");

            self.changeTab($this.attr("level"));
        });

        this.$(".form-inline").bind("keypress", function (e) {
            if (e.keyCode == 13) {
                return false;
            }
        });

        if (this.reportParam.SearchRoleId > 0) {
            this.$(".lnk-changetab[level='" + this.reportParam.SearchRoleId + "']").click();
            switch (this.reportParam.SearchRoleId) {
                case siteInfrastructure.roleId.master:
                    this.masterList.reloadState(this.reportParam);
                    break;
                case siteInfrastructure.roleId.agent:
                    this.$("#btn-submit-master").click();
                    this.agentList.reloadState(this.reportParam);
                    break;
                case siteInfrastructure.roleId.member:
                    this.$("#btn-submit-master").click();

                    let memberUsername = location.search.split(/[?&]/)[1].split('=')[1];
                    let agentUsername = memberUsername.substr(0, memberUsername.length - 2);
                    this.agentList.$viewedDirectUpline.val(agentUsername);
                    this.$("#btn-submit-agent").click();

                    this.memberList.reloadState(this.reportParam);
                    break;
            }
        } else {
            switch (parseInt(directDownline)) {
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
    };

    cacheDOMElements = () => {
        this.$navTabs = this.$("#nav-tabs");

        this.AccessDenyPage = this.$("#AccessDenyPage").val();

        this.AccountManagementEditView = this.$("#AccountManagementEditView").val() === "1";
        this.Language = this.$("#Language").val();
        this.ViewerId = parseInt(this.$("#ViewerId").val());
        this.ActorLevel = parseInt(this.$("#ActorLevel").val());
        this.isDevice = this.$("#IsDevice").val() == "1";

        this.reportParam = {
            DownlineUsername: this.$("#DownlineUsername").val(),
            Username: <string>this.$("#Username").val(),
            SearchRoleId: parseInt(<string>this.$("#SearchRoleId").val()),
            CustId: 0,
            Status: parseInt(<string>this.$("#Status").val()),
            PageSize: parseInt(<string>this.$("#PageSize").val()),
            PageIndex: 1
        };
    };

    changeTab = function (level: string) {
        this.$(".tab-content").addClass("display-none");
        this.$(`.tab-content[level='${level}']`).removeClass("display-none");
    };

    updateCredit = (custId: any, username: any, custLevelId: any) => {
        const updateCreditLink = this.$("#UpdateCreditLink").val() + '?' + this.$.param({ custId: custId, custLevelId: custLevelId }),
            popupTitle = `${this.$('#UpdateCredit').val()} - ${username}`;

        this.openPopup(popupTitle, updateCreditLink, 'modal modal-credit', 490, 280);
    };

    updateStatus = (custId: string, username: string, custLevelId: any, statusId: string, uplineStatusId: string) => {
        const updateStatusLink = this.$("#UpdateStatusLink").val() + '?' + this.$.param({ custId: custId, statusId: statusId, custLevelId: custLevelId, uplineStatusId: uplineStatusId }),
            popupTitle = `${this.$('#UpdateStatus').val()} - ${username}`;

        this.openPopup(popupTitle, updateStatusLink, 'modal modal-status', 490, 210);
    };

    resetPassword = (custId: string, username: string, custLevelId: string) => {
        const resetPasswordLink = `${this.$("#ResetPasswordLink").val()}?custId=${custId}&username=${username}&custlevelid=${custLevelId}`,
            popupTitle: string = `${this.$('#ResetPassword').val()} - ${username}`;

        this.openPopup(popupTitle, resetPasswordLink, 'modal modal-password', 490, 430);
    };

    resetSecurityCode = (custId: string, username: string) => {
        const resetSecurityCodeLink = `${this.$("#ResetSecurityCodeLink").val()}?custId=${custId}`,
            popupTitle: string = `${this.$('#ResetSecurityCode').val()} - ${username}`;

        this.openPopup(popupTitle, resetSecurityCodeLink, 'modal modal-password', 490, 344);
    };

    openPopup = (popupTitle: string, url: string, modifiedClass: string, width: number = 480, height: number = 300) => {
        this.topWindow.popupManager.init(this.$, this.topWindow);
        this.topWindow.popupManager.open({
            title: popupTitle,
            url: url,
            width: width,
            height: height,
            popupCss: modifiedClass
        });
    };

    updateAccount = (custId: any, custLevelId: any, reportParam: any) => {
        this.storeCurrentPage(reportParam);
        this.topWindow.showCN88CancelButton = true;
        this.topWindow.showCN88SaveAndNewButton = false;

        const editCustomerLink = this.$("#EditCustomerLink").val();
        window.location.href = editCustomerLink + "?" + this.$.param({
            Language: this.Language,
            ProductId: 1,
            CustomerLevel: custLevelId,
            CustomerId: custId,
            IsReadOnly: !this.AccountManagementEditView,
            UplineId: this.ViewerId,
            ActorLevel: this.ActorLevel
        });
    };

    updateUnlock = (custId: string, username: string) => {
        this.topWindow.popupManager.init(this.$, this.topWindow);
        this.topWindow.popupManager.bootboxConfirm(
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
            350, this.viewResources.Ok, this.viewResources.Cancel
        );
    };

    completedUnlock(params: any) {
        const { custId, $this } = params;
        $this.isDevice ?
            $this.$(`tr[cust-id='${custId}']`).find('.icon-user-unlock').removeClass().addClass('icon-more-action') :
            $this.$(`.icon-unlock[cust-id='${custId}']`).remove();
    };

    addNewDownline = (custId: any, username: any, custLevelId: any, reportParam: ReportParam, downlineUsername: string = '') => {
        const mainiFrame: JQuery = this.topWindow.$("#main");
        if (this.AccountManagementEditView) {
            reportParam.CustId = custId;
            reportParam.Username = username;
            reportParam.SearchRoleId = custLevelId;
            reportParam.DownlineUsername = downlineUsername;

            this.storeCurrentPage(reportParam);
            this.topWindow.showCN88CancelButton = true;
            this.topWindow.showCN88SaveAndNewButton = true;

            const createCustomerLink = this.$("#CreateCustomerLink").val();
            mainiFrame.show();
            mainiFrame.attr("src", createCustomerLink + "?" + this.$.param({
                Language: this.Language,
                ProductId: 1,
                CustomerLevel: custLevelId,
                CustomerId: custId,
                IsReadOnly: !this.AccountManagementEditView,
                UplineId: custId,
                ActorLevel: this.ActorLevel
            }));
        } else {
            mainiFrame.attr("src", this.AccessDenyPage);
        }
    };

    getStatusName(status: number) {
        switch (status) {
            case siteInfrastructure.customerStatus.active:
                return this.viewResources.Active;
            case siteInfrastructure.customerStatus.viewOnly:
                return this.viewResources.ViewOnly;
            case siteInfrastructure.customerStatus.suspended:
                return this.viewResources.Suspended;
            case siteInfrastructure.customerStatus.inactive:
                return this.viewResources.Inactive;
            default: return '';
        }
    };

    storeCurrentPage(reportParam: any) {
        const currentPage = this.$("#CurrentLink").val() + "?" + this.$.param(reportParam);
        siteInfrastructure.localStorage.set("reloadCN88CustomerListCallBackUrl", { currentPage: currentPage });
    };

    reloadData = (levelName: string) => {
        const timeoutDefault = 2000;

        setTimeout(() => {
            const submitBtn = this.$(`input[id^='btn-submit-${levelName}']`)[0];
            if (submitBtn !== undefined) {
                submitBtn.click();
            }
        }, timeoutDefault);
    };
};

$(() => {
    new CustomerList($, numeral,  window.top).init();
});