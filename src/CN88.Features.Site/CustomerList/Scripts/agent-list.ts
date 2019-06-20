/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="../../shared/scripts/components/pager.ts" />
/// <reference path="base-list.ts" />

declare var initAutoComplete: Function;

class AgentList extends BaseList {
    $viewedMaster: JQuery;

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
    };

    viewAgents(custId: number) {
        this.viewList(this.$viewedMaster.val(), custId);
        const directDownline = parseInt(<string>this.$("#directDownline").val()),
            $errAgent = this.$("#msg-err-agent"),
            { DownlineUsername } = this.reportParam;

        if (directDownline != siteInfrastructure.roleId.agent &&
            (DownlineUsername == "" || DownlineUsername == undefined)) {
            $errAgent.removeClass("display-none");
            this.$.unblockUI();
            return;
        }

        $errAgent.addClass("display-none");
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