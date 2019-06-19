/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="base-list.ts" />
/// <reference path="../../shared/scripts/components/pager.ts" />

class MemberList extends BaseList {
    $viewedAgent: JQuery;
    $errMember: JQuery;
    directDownline: number;

    $output: JQuery;
    usernameAutoSuggestionPath: string;

    constructor(
        $: JQueryStatic,
        statusComponent: SingleSelectionDropdownComponent,
        bootstrapDropdownComponent: BootstrapDropdownComponent,
        numeral: any,
        pager: PagerComponent) {
        super($, statusComponent, bootstrapDropdownComponent, numeral, pager, "Member");
    }

    public init(viewResources: any, indexManager: any) {
        super.init(
            viewResources,
            indexManager,
            this.renderMemberList,
            siteInfrastructure.roleId.member,
            null,
            indexManager.addNewDownline);

        this.cacheDOMElements();

        let self = this;
        this.$("#btn-submit-member").off("click").on("click", function () {
            self.reportParam.PageIndex = 1;
            self.viewMembers(self.reportParam.CustId);
        });
        if (this.directDownline >= siteInfrastructure.roleId.agent) {
            //this.initAutoComplete();
        }
    }

    public cacheDOMElements() {
        super.cacheDOMElements();

        this.$viewedAgent = this.$("#txt-viewed-agent");
        this.directDownline = parseInt(this.$("#directDownline").val());
        this.$errMember = this.$("#msg-err-member");
        this.usernameAutoSuggestionPath = this.$("#usernameAutoSuggestionPath").val();
        this.$output = this.$("#output");
    }
    viewMembers = (custId: number) => {
        this.$.blockUI();
        this.reportParam = {
            DownlineUsername: this.$("#txt-viewed-agent").val(),
            Username: this.$username.val(),
            SearchRoleId: siteInfrastructure.roleId.member,
            CustId: custId,
            Status: this.$dropdownStatus.find("option:selected").val(),
            PageSize: this.reportParam.PageSize,
            PageIndex: this.reportParam.PageIndex
        };

        if (this.directDownline != siteInfrastructure.roleId.member &&
            (this.reportParam.DownlineUsername == "" ||
                this.reportParam.DownlineUsername == undefined)) {
            this.$errMember.removeClass("display-none");
            this.$.unblockUI();
            return;
        }

        this.$errMember.addClass("display-none");

        if (this.indexManager.isDevice) {
            this.indexManager.memberToggleHandler.hideFilter();
        }

        this.loadData();
    };
    renderMemberList = (rs: any) => {
        this.renderCustomerList(rs, 1, 'member');
    };
    viewMembersOfAgent = (custId: any, username: any, status: number) => {
        this.$viewedAgent.val(username);
        this.$username.val("");

        this.reportParam.PageIndex = 1;
        this.reportParam.PageSize = 100;
        this.reportParam.Status = status;

        this.statusComponent.changeSelection(status);

        this.viewMembers(custId);
    };
    initAutoComplete = () => {
        let queryUsernameURL = this.usernameAutoSuggestionPath + "?" +
            this.$.param({ isDir: this.directDownline == siteInfrastructure.roleId.agent, issub: false, ismember: true });
        //initAutoComplete(this.$viewedAgent[0], queryUsernameURL);

        this.$output.css("top", this.$viewedAgent.offset().top + 30);
        this.$output.css("left", this.$viewedAgent.offset().left);
    };
    reloadState = (reportParam: any) => {
        this.$("#txt-viewed-agent").val(reportParam.DownlineUsername);
        this.$username.val(reportParam.Username);
        this.$dropdownStatus.val(this.$("#dropdown-status-member").val());
        this.reportParam.PageSize = reportParam.PageSize;
        this.reportParam.PageIndex = reportParam.PageIndex;

        this.$("#PageIndexMember").val(reportParam.PageIndex);
        this.$("#PageSizeMember").val(reportParam.PageIndex);

        this.viewMembers(reportParam.CustId);
    };
}