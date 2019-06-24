/// <reference path="../../shared/scripts/components/filter-toggle.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="../../shared/scripts/components/pager.ts" />
/// <reference path="master-list.ts" />
/// <reference path="agent-list.ts" />
/// <reference path="member-list.ts" />
/// <reference path="viewmodel/view-model.ts" />

class CustomerList {
    $: JQueryStatic;
    topWindow: any;
    numeral: any;
    statusComponent: SingleSelectionDropdownComponent;
    pager: PagerComponent;
    masterToggleHandler: ToggleHandler;
    agentToggleHandler: ToggleHandler;
    memberToggleHandler: ToggleHandler;

    $navTabs: JQuery;
    AccessDenyPage: string;
    isDevice: boolean = false;

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

    masterList: MasterList;
    agentList: AgentList;
    memberList: MemberList;
    AccountManagementEditView: boolean;
    Language: string;
    ViewerId: string;
    ActorLevel: string;

    constructor(
        $: JQueryStatic,
        statusComponent: SingleSelectionDropdownComponent,
        numeral: any,
        pager: PagerComponent,
        topWindow: any,
        masterToggleHandler: ToggleHandler,
        agentToggleHandler: ToggleHandler,
        memberToggleHandler: ToggleHandler,
        viewResources:ViewResources) {
        this.$ = $;
        this.topWindow = topWindow;
        this.statusComponent = statusComponent;
        this.numeral = numeral;
        this.pager = pager;
        this.viewResources = viewResources;

        this.masterToggleHandler = masterToggleHandler;
        this.agentToggleHandler = agentToggleHandler;
        this.memberToggleHandler = memberToggleHandler;

        this.masterList = new MasterList($, new SingleSelectionDropdownComponent($), new BootstrapDropdownComponent($, topWindow), numeral, new PagerComponent($));
        this.agentList = new AgentList($, new SingleSelectionDropdownComponent($), new BootstrapDropdownComponent($, topWindow), numeral, new PagerComponent($));
        this.memberList = new MemberList($, new SingleSelectionDropdownComponent($), new BootstrapDropdownComponent($, topWindow), numeral, new PagerComponent($));
    }

    init = () => {
        this.cacheDOMElements();

        this.masterList.init(this);
        this.agentList.init(this);
        this.memberList.init(this);

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
                    this.agentList.$viewedMaster.val(agentUsername);
                    this.$("#btn-submit-agent").click();

                    this.memberList.reloadState(this.reportParam);
                    break;
            }
        } else {
            switch (parseInt(directDownline)) {
                case siteInfrastructure.roleId.master:
                    this.masterList.viewMasters();
                    break;
                case siteInfrastructure.roleId.agent:
                    this.agentList.viewAgents(this.reportParam.CustId);
                    break;
                case siteInfrastructure.roleId.member:
                    this.memberList.viewMembers(this.reportParam.CustId);
                    break;
            }
        }

        this.isDevice = this.$("#IsDevice").val() == "1";
        if (this.isDevice) {
            this.masterToggleHandler.render(new ToggleHandlerOption("master-filter-toggle-icon", "form-master", "master-container"));
            this.agentToggleHandler.render(new ToggleHandlerOption("agent-filter-toggle-icon", "form-agent", "agent-container"));
            this.memberToggleHandler.render(new ToggleHandlerOption("member-filter-toggle-icon", "form-member", "member-container"));
        }
    };

    cacheDOMElements = () => {
        this.$navTabs = this.$("#nav-tabs");

        this.AccessDenyPage = this.$("#AccessDenyPage").val();

        this.AccountManagementEditView = this.$("#AccountManagementEditView").val() === "1";
        this.Language = this.$("#Language").val();
        this.ViewerId = this.$("#ViewerId").val();
        this.ActorLevel = this.$("#ActorLevel").val();

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
        const updateCreditLink = this.$("#UpdateCreditLink").val();
        let url = updateCreditLink + '?' + this.$.param({ custId: custId, custLevelId: custLevelId }),
            popupTitle = `${this.$('#UpdateCredit').val()} - ${username}`;

        this.openPopup(popupTitle, url, 'modal modal-credit', 490, 280);
    };

    updateStatus = (custId: number, username: string, custLevelId: any, statusId: number, uplineStatusId: number) => {
        const updateStatusLink = this.$("#UpdateStatusLink").val();
        let url = updateStatusLink + '?' + this.$.param({ custId: custId, statusId: statusId, custLevelId: custLevelId, uplineStatusId: uplineStatusId }),
            popupTitle = `${this.$('#UpdateStatus').val()} - ${username}`;

        this.openPopup(popupTitle, url, 'modal modal-status', 490, 210);
    };

    resetPassword = (custId: number, username: string, custLevelId: number) => {
        const resetPasswordLink = this.$("#ResetPasswordLink").val();
        let url: string = `${resetPasswordLink}?custId=${custId}&username=${username}&custlevelid=${custLevelId}`;
        let popupTitle: string = `${this.$('#ResetPassword').val()} - ${username}`;

        this.openPopup(popupTitle, url, 'modal modal-password', 490, 430);
    };

    resetSecurityCode = (custId: number, username: string) => {
        const resetSecurityCodeLink = this.$("#ResetSecurityCodeLink").val();
        let url: string = `${resetSecurityCodeLink}?custId=${custId}`;
        let popupTitle: string = `${this.$('#ResetSecurityCode').val()} - ${username}`;

        this.openPopup(popupTitle, url, 'modal modal-password', 490, 344);
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

    updateAccount = (custId: any, username: any, custLevelId: any, reportParam: any) => {
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

    updateUnlock = (custId: any) => {
        let self = this;
        this.topWindow.popupManager.init(this.$, this.topWindow);
        this.topWindow.popupManager.bootboxConfirm(
            this.viewResources.UnlockConfirmTitle,
            this.viewResources.UnlockConfirmMessage,
            () => {
                self.$.ajax({
                    type: "POST",
                    url: 'UnlockCustomer',
                    data: { "CustId": custId},
                    dataType: "json",
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("__RequestVerificationToken", self.$("input[name=\"__RequestVerificationToken\"]").val());
                    }})
                    .done(function (data) {
                        if (data.IsSuccess) {
                            self.$(`.icon-unlock[cust-id='${custId}']`).remove();
                        }
                        else {
                            self.topWindow.popupManager.bootboxAlert(self.viewResources.UnlockConfirmTitle, data.Message);
                        }
                    })
                    .fail(function (jqXHR: JQueryXHR) {
                        if (jqXHR.status === 403) {
                            self.topWindow.popupManager.bootboxAlert(jqXHR.responseJSON.message);
                        }
                    });
            }
        );
    };

    addNewDownline = (custId: any, username: any, custLevelId: any, reportParam: any, downlineUsername: string = '') => {
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
    getStatusName = (status: number) => {
        if (status == siteInfrastructure.customerStatus.active) {
            return this.viewResources.Active;
        }

        if (status == siteInfrastructure.customerStatus.viewOnly) {
            return this.viewResources.ViewOnly;
        }

        if (status == siteInfrastructure.customerStatus.suspended) {
            return this.viewResources.Suspended;
        }

        if (status == siteInfrastructure.customerStatus.inactive) {
            return this.viewResources.Inactive;
        }

        return "";
    };
    viewMaster = (custId: any, username: any, status: number) => {
        this.$(`.lnk-changetab[level='${siteInfrastructure.roleId.agent}']`).click();
        this.agentList.viewAgentsOfMaster(custId, username, status);
    };
    viewAgent = (custId: any, username: any, status: number) => {
        this.$(`.lnk-changetab[level='${siteInfrastructure.roleId.member}']`).click();
        this.memberList.viewMembersOfAgent(custId, username, status);
    };
    removeUnlock = (custId: any) => {
        this.$(`.icon-unlock[cust-id='${custId}']`).remove();
    };
    storeCurrentPage = (reportParam: any) => {
        const currentLink = this.$("#CurrentLink").val();
        let currentPage = currentLink + "?" + this.$.param(reportParam);
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

const customerList = new CustomerList(
    $,
    new SingleSelectionDropdownComponent($),
    numeral,
    new PagerComponent($),
    window.top,
    new ToggleHandler($),
    new ToggleHandler($),
    new ToggleHandler($),
    new ViewResources($));

$(() => {
    customerList.init();
});