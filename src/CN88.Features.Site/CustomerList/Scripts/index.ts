class CustomerList {
    $: JQueryStatic;
    topWindow: any;
    numeral: any;
    statusComponent: SingleSelectionDropdownComponent;
    pager: PagerComponent;

    directDownline: string;
    $navTabs: JQuery;
    EditCustomerLink: string;
    CreateCustomerLink: string;
    UpdateCreditLink: string;
    UpdateStatusLink: string;

    CurrentLink: string;

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
        Disabled: ''
    };

    masterList: MasterList;
    agentList: AgentList;
    memberList: MemberList;
    HasUpdatePermission: boolean;
    HasAddNewPermission: boolean;
    Language: string;
    ViewerId: string;
    ActorLevel: string;

    constructor(
        $: JQueryStatic,
        statusComponent: SingleSelectionDropdownComponent,
        numeral: any,
        pager: PagerComponent,
        topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
        this.statusComponent = statusComponent;
        this.numeral = numeral;
        this.pager = pager;

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

        var self = this;
        this.$(".lnk-changetab").click(function () {
            var $this = self.$(this);
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
                    this.agentList.reloadState(this.reportParam);
                    break;
                case siteInfrastructure.roleId.member:
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

        this.EditCustomerLink = this.$("#EditCustomerLink").val();
        this.CreateCustomerLink = this.$("#CreateCustomerLink").val();
        this.CurrentLink = this.$("#CurrentLink").val();

        this.HasUpdatePermission = this.$("#HasUpdatePermission").val() == "1";
        this.HasAddNewPermission = this.$("#HasAddNewPermission").val() == "1";
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
            case siteInfrastructure.roleId.agent:
                this.agentList.initAutoComplete();
                break;
            case siteInfrastructure.roleId.member:
                this.memberList.initAutoComplete();
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

        this.openPopup(popupTitle, url, 'modal modal-credit');
    };

    updateStatus = (custId: any, username: any, updateStatusId: any) => {
        let url = this.UpdateStatusLink + '?' + this.$.param({ custId: custId, UpdateStatusId: updateStatusId }),
            popupTitle = `${this.$('#UpdateStatus').val()} - ${username}`;

        this.openPopup(popupTitle, url, 'modal modal-status');
    };

    resetPassword = (custId: number, username: string) => {
        const url = `Password/ResetDownlinePassword?custId=${custId}&username=${username}`;
        const popupTitle = `${this.$('#ResetPassword').val()} - ${username}`;

        this.openPopup(popupTitle, url, 'modal modal-password');
    };

    openPopup = (popupTitle: string, url: string, modifiedClass: string) => {
        let self = this;

        this.topWindow.popupManager.init(this.$, this.topWindow);
        this.topWindow.popupManager.open({
            title: popupTitle,
            url: url,
            width: 480,
            height: 300,
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
            IsReadOnly: !this.HasUpdatePermission,
            UplineId: this.ViewerId,
            ActorLevel: this.ActorLevel
        });
    };
    addNewDownline = (custId: any, username: any, custLevelId: any, reportParam: any, downlineUsername: string = '') => {
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
            IsReadOnly: !this.HasAddNewPermission,
            UplineId: custId,
            ActorLevel: this.ActorLevel
        });
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
        var column = this.$(".col-status[cust-id='" + custId + "']");
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
        var currentPage = this.CurrentLink + "?" + this.$.param(reportParam);
        var $currentPage = this.topWindow.$("body #reloadCN88CustomerListCallBackUrl");

        if ($currentPage.length == 0) {
            this.topWindow.$("body").append("<input id='reloadCN88CustomerListCallBackUrl' type='hidden' value='' />");
        }

        this.topWindow.$("body #reloadCN88CustomerListCallBackUrl").val(currentPage);
    };
};

const customerList = new CustomerList(
    $,
    new SingleSelectionDropdownComponent($),
    numeral,
    new PagerComponent($),
    window.top);

$(() => {
    customerList.init();
});
