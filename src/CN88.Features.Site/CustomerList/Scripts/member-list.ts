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
        this.directDownline = parseInt(this.$("#directDownline").val());
        this.$errMember = this.$("#msg-err-member");
        this.usernameAutoSuggestionPath = this.$("#usernameAutoSuggestionPath").val();
        this.$output = this.$("#output");
    }
    viewMembers(custId: number) {
        this.viewList(this.$("#txt-viewed-agent").val(), custId);

        if (this.directDownline != siteInfrastructure.roleId.member &&
            (this.reportParam.DownlineUsername == "" ||
                this.reportParam.DownlineUsername == undefined)) {
            this.$errMember.removeClass("display-none");
            this.$.unblockUI();
            return;
        }

        this.$errMember.addClass("display-none");
    };
    viewMembersOfAgent(custId: number, username: string, status: number) {
        this.$viewedAgent.val(username);
        this.setStateWhenViewDownlines(status);
        this.viewMembers(custId);
    };
    reloadState(reportParam: any){
        this.$("#txt-viewed-agent").val(reportParam.DownlineUsername);
        this.baseReloadState(reportParam, 'Member');
        this.viewMembers(reportParam.CustId);
    };
}