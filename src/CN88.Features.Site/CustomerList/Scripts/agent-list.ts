/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="../../shared/scripts/components/pager.ts" />
/// <reference path="base-list.ts" />

declare var initAutoComplete: Function;

class AgentList extends BaseList {
    $viewedMaster: JQuery;
    $errAgent: JQuery;
    directDownline: number;

    $output: JQuery;
    usernameAutoSuggestionPath: string;

    constructor(
        $: JQueryStatic,
        statusComponent: SingleSelectionDropdownComponent,
        bootstrapDropdownComponent: BootstrapDropdownComponent,
        numeral: any,
        pager: PagerComponent) {
        super($, statusComponent, bootstrapDropdownComponent, numeral, pager, "Agent");
    };

    public init(indexManager: any) {
        super.init(
            indexManager,
            siteInfrastructure.roleId.agent,
            indexManager.viewAgent);

        this.cacheDOMElements();

        this.$("#btn-submit-agent").off("click").on("click", () => {
            this.reportParam.PageIndex = 1;
            this.viewAgents(this.reportParam.CustId);
        });
    };

    public cacheDOMElements() {
        super.cacheDOMElements();

        this.$viewedMaster = this.$("#txt-viewed-master");
        this.directDownline = parseInt(<string>this.$("#directDownline").val());
        this.$errAgent = this.$("#msg-err-agent");
        this.usernameAutoSuggestionPath = this.$("#usernameAutoSuggestionPath").val();
        this.$output = this.$("#output");
    };

    viewAgents(custId: number) {
        this.viewList(this.$viewedMaster.val(), custId);

        if (this.directDownline != siteInfrastructure.roleId.agent &&
            (this.reportParam.DownlineUsername == "" ||
                this.reportParam.DownlineUsername == undefined)) {
            this.$errAgent.removeClass("display-none");
            this.$.unblockUI();
            return;
        }

        this.$errAgent.addClass("display-none");
    };
    viewAgentsOfMaster(custId: number, username: string, status: number) {
        this.$viewedMaster.val(username);
        this.setStateWhenViewDownlines(status);
        this.viewAgents(custId);
    };
    reloadState(reportParam: any) {
        this.$viewedMaster.val(reportParam.DownlineUsername);
        this.baseReloadState(reportParam, 'Agent');
        this.viewAgents(reportParam.CustId);
    };
}