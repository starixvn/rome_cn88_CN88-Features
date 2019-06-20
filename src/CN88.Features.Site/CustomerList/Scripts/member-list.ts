/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="base-list.ts" />
/// <reference path="../../shared/scripts/components/pager.ts" />

class MemberList extends BaseList {
    $viewedAgent: JQuery;

    constructor(
        $: JQueryStatic,
        statusComponent: SingleSelectionDropdownComponent,
        bootstrapDropdownComponent: BootstrapDropdownComponent,
        numeral: any,
        pager: PagerComponent) {
        super($, statusComponent, bootstrapDropdownComponent, numeral, pager, "Member");
    }

    public init(indexManager: any) {
        super.init(
            indexManager,
            siteInfrastructure.roleId.member,
            null);

        this.cacheDOMElements();

        this.$("#btn-submit-member").off("click").on("click", () => {
            this.reportParam.PageIndex = 1;
            this.viewMembers(this.reportParam.CustId);
        });
    }

    public cacheDOMElements() {
        super.cacheDOMElements();
        this.$viewedAgent = this.$("#txt-viewed-agent");
    }
    viewMembers(custId: number) {
        this.viewList(this.$viewedAgent.val(), custId);
        const directDownline = parseInt(this.$("#directDownline").val()),
            $errMember = this.$("#msg-err-member"),
            { DownlineUsername } = this.reportParam;

        if (directDownline != siteInfrastructure.roleId.member &&
            (DownlineUsername == "" || DownlineUsername == undefined)) {
            $errMember.removeClass("display-none");
            this.$.unblockUI();
            return;
        }

        $errMember.addClass("display-none");
    };
    viewMembersOfAgent(custId: number, username: string, status: number) {
        this.$viewedAgent.val(username);
        this.setStateWhenViewDownlines(status);
        this.viewMembers(custId);
    };
    reloadState(reportParam: any){
        this.$viewedAgent.val(reportParam.DownlineUsername);
        this.baseReloadState(reportParam, 'Member');
        this.viewMembers(reportParam.CustId);
    };
}