class CustomerList {
    $: JQueryStatic;
    topWindow: any;
    numeral: any;
    statusComponent: SingleSelectionDropdownComponent;
    pager: PagerComponent;
    masterToggleHandler: ToggleHandler;
    agentToggleHandler: ToggleHandler;
    memberToggleHandler: ToggleHandler;

    directDownline: string;
    $navTabs: JQuery;
    EditCustomerLink: string;
    CreateCustomerLink: string;
    UpdateCreditLink: string;
    UpdateStatusLink: string;
    AccessDenyPage: string;
    isDevice: boolean = false;

    CurrentLink: string;
    ResetPasswordLink: string;
    ResetSecurityCodeLink: string;

    reportParam = {
        DownlineUsername: null,
        Username: "",
        SearchRoleId: 0,
        CustId: 0,
        Status: 0,
        PageSize: 100,
        PageIndex: 1
    };

    viewResources = {
        NoInformationIsAvailable: '',
        Open: '',
        Closed: '',
        Suspended: '',
        Page: '',
        Of: '',
        Pager_DisplayItems: '',
        ResetSecurityCodeDisabled: '',
        Disabled: '',
        NewMember: '',
        NewAgent: '',
        EditMember: '',
        EditAgent: '',
        EditMasterAgent: '',
        ResetPassword: '',
        ResetSecurityCode: ''
    };

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
        memberToggleHandler: ToggleHandler) {
        this.$ = $;
        this.topWindow = topWindow;
        this.statusComponent = statusComponent;
        this.numeral = numeral;
        this.pager = pager;
        this.masterToggleHandler = masterToggleHandler;
        this.agentToggleHandler = agentToggleHandler;
        this.memberToggleHandler = memberToggleHandler;

        this.masterList = new MasterList($, new SingleSelectionDropdownComponent($), numeral, new PagerComponent($));
        this.agentList = new AgentList($, new SingleSelectionDropdownComponent($), numeral, new PagerComponent($));
        this.memberList = new MemberList($, new SingleSelectionDropdownComponent($), numeral, new PagerComponent($));
    }

    init = () => {
        this.cacheDOMElements();

        this.masterList.init(this.viewResources, this);
        this.agentList.init(this.viewResources, this);
        this.memberList.init(this.viewResources, this);

        this.$navTabs.find("li[level='" + this.directDownline + "']").addClass("active");
        this.$(".tab-content[level='" + this.directDownline + "']").removeClass("display-none");

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
            switch (parseInt(this.directDownline)) {
                case siteInfrastructure.roleId.master:
                    this.masterList.viewList();
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
        this.directDownline = this.$("#directDownline").val();
        this.$navTabs = this.$("#nav-tabs");

        this.viewResources.NoInformationIsAvailable = this.$("#NoInformationIsAvailable").val();
        this.viewResources.Open = this.$("#Open").val();
        this.viewResources.Closed = this.$("#Closed").val();
        this.viewResources.Suspended = this.$("#Suspended").val();

        this.viewResources.Page = this.$("#Page").val();
        this.viewResources.Of = this.$("#of").val();
        this.viewResources.Pager_DisplayItems = this.$("#Pager_DisplayItems").val();
        this.viewResources.Disabled = this.$("#Disabled").val();

        this.viewResources.NewMember = this.$("#NewMember").val();
        this.viewResources.NewAgent = this.$("#NewAgent").val();
        this.viewResources.EditMember = this.$("#EditMember").val();
        this.viewResources.EditAgent = this.$("#EditAgent").val();
        this.viewResources.EditMasterAgent = this.$("#EditMasterAgent").val();

        this.viewResources.ResetPassword = this.$("#ResetPassword").val();
        this.viewResources.ResetSecurityCode = this.$("#ResetSecurityCode").val();

        this.EditCustomerLink = this.$("#EditCustomerLink").val();
        this.CreateCustomerLink = this.$("#CreateCustomerLink").val();
        this.CurrentLink = this.$("#CurrentLink").val();
        this.ResetPasswordLink = this.$("#ResetPasswordLink").val();
        this.ResetSecurityCodeLink = this.$("#ResetSecurityCodeLink").val();
        this.AccessDenyPage = this.$("#AccessDenyPage").val();

        this.AccountManagementEditView = this.$("#AccountManagementEditView").val() === "1";
        this.Language = this.$("#Language").val();
        this.ViewerId = this.$("#ViewerId").val();
        this.ActorLevel = this.$("#ActorLevel").val();

        this.UpdateCreditLink = this.$("#UpdateCreditLink").val();
        this.UpdateStatusLink = this.$("#UpdateStatusLink").val();

        this.reportParam = {
            DownlineUsername: this.$("#DownlineUsername").val(),
            Username: this.$("#Username").val(),
            SearchRoleId: parseInt(this.$("#SearchRoleId").val()),
            CustId: 0,
            Status: parseInt(this.$("#Status").val()),
            PageSize: parseInt(this.$("#PageSize").val()),
            PageIndex: 1
        };
    };
    changeTab = function (level: string) {
        this.$(".tab-content").addClass("display-none");
        this.$(".tab-content[level='" + level + "']").removeClass("display-none");

        switch (parseInt(level)) {
            case siteInfrastructure.roleId.master:
                //this.masterList.initAutoComplete();
                break;
            case siteInfrastructure.roleId.agent:
                //this.agentList.initAutoComplete();
                break;
            case siteInfrastructure.roleId.member:
                //this.memberList.initAutoComplete();
                break;
        }
    };

    getCustomerColor = (customer: any) => {
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

    updateCredit = (custId: any, username: any, custLevelId: any) => {
        let url = this.UpdateCreditLink + '?' + this.$.param({ custId: custId, custLevelId: custLevelId }),
            popupTitle = `${this.$('#UpdateCredit').val()} - ${username}`;

        this.openPopup(popupTitle, url, 'modal modal-credit', 490, 280);
    };

    updateStatus = (custId: any, username: any, updateStatusId: any) => {
        let url = this.UpdateStatusLink + '?' + this.$.param({ custId: custId, UpdateStatusId: updateStatusId }),
            popupTitle = `${this.$('#UpdateStatus').val()} - ${username}`;

        this.openPopup(popupTitle, url, 'modal modal-status', 490, 210);
    };

    resetPassword = (custId: number, username: string, custLevelId: number) => {
        let url: string = `${this.ResetPasswordLink}?custId=${custId}&username=${username}&custlevelid=${custLevelId}`;
        let popupTitle: string = `${this.$('#ResetPassword').val()} - ${username}`;

        this.openPopup(popupTitle, url, 'modal modal-password', 490, 430);
    };

    resetSecurityCode = (custId: number, username: string) => {
        let url: string = `${this.ResetSecurityCodeLink}?custId=${custId}`;
        let popupTitle: string = `${this.$('#ResetSecurityCode').val()} - ${username}`;

        this.openPopup(popupTitle, url, 'modal modal-password', 490, 344);
    };

    openPopup = (popupTitle: string, url: string, modifiedClass: string, width: number = 480, height: number = 300) => {
        let self = this;

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

        window.location.href = this.EditCustomerLink + "?" + this.$.param({
            Language: this.Language,
            ProductId: 1,
            CustomerLevel: custLevelId,
            CustomerId: custId,
            IsReadOnly: !this.AccountManagementEditView,
            UplineId: this.ViewerId,
            ActorLevel: this.ActorLevel
        });
    };
    addNewDownline = (custId: any, username: any, custLevelId: any, reportParam: any, downlineUsername: string = '') => {
        if (this.AccountManagementEditView) {
            reportParam.CustId = custId;
            reportParam.Username = username;
            reportParam.SearchRoleId = custLevelId;
            reportParam.DownlineUsername = downlineUsername;

            this.storeCurrentPage(reportParam);
            this.topWindow.showCN88CancelButton = true;

            window.location.href = this.CreateCustomerLink + "?" + this.$.param({
                Language: this.Language,
                ProductId: 1,
                CustomerLevel: custLevelId,
                CustomerId: custId,
                IsReadOnly: !this.AccountManagementEditView,
                UplineId: custId,
                ActorLevel: this.ActorLevel
            });
        } else {
            window.location.href = this.AccessDenyPage;
        }
    };
    getStatusName = (status: number) => {
        if (status == siteInfrastructure.customerStatus.open) {
            return this.viewResources.Open;
        }

        if (status == siteInfrastructure.customerStatus.closed) {
            return this.viewResources.Closed;
        }

        if (status == siteInfrastructure.customerStatus.suspended) {
            return this.viewResources.Suspended;
        }

        return "";
    };
    viewMaster = (custId: any, username: any) => {
        this.$(".lnk-changetab[level='" + siteInfrastructure.roleId.agent + "']").click();
        this.agentList.viewAgentsOfMaster(custId, username);
    };
    viewAgent = (custId: any, username: any) => {
        this.$(".lnk-changetab[level='" + siteInfrastructure.roleId.member + "']").click();
        this.memberList.viewMembersOfAgent(custId, username);
    };
    reloadCreditValue = (custId: any, newValue: any) => {
        this.$(".col-credit[cust-id='" + custId + "']").html(this.numeral(newValue).format("###,0.00"));
    };
    reloadStatusValue = (custId: any, newValue: any) => {
        let column = this.$(".col-status[cust-id='" + custId + "']");
        column.html(this.getStatusName(newValue));
        column.attr("current-status", newValue);

        this.$(".row-customer[cust-id='" + custId + "']")
            .removeClass()
            .addClass("row-customer " + this.reloadStatusColor(newValue));
    };
    reloadStatusColor = (status: any) => {
        if (status == siteInfrastructure.customerStatus.open) {
            return "row-open";
        }

        if (status == siteInfrastructure.customerStatus.suspended) {
            return "row-suspended";
        }

        return "row-closed reason-" + siteInfrastructure.closeReasonConstant.Upline;
    };
    storeCurrentPage = (reportParam: any) => {
        let currentPage = this.CurrentLink + "?" + this.$.param(reportParam);
        siteInfrastructure.localStorage.set("reloadCN88CustomerListCallBackUrl", { currentPage: currentPage });
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
    new ToggleHandler($));

$(() => {
    customerList.init();
});