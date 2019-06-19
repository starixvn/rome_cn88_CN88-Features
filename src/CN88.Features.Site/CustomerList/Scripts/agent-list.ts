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
    }

    public init(viewResources: any, indexManager: any) {
        super.init(
            viewResources,
            indexManager,
            this.renderAgentList,
            siteInfrastructure.roleId.agent,
            indexManager.viewAgent,
            indexManager.addNewDownline);

        this.cacheDOMElements();

        var self = this;
        this.$("#btn-submit-agent").off("click").on("click", function () {
            self.reportParam.PageIndex = 1;
            self.viewAgents(self.reportParam.CustId);
        });
    }

    public cacheDOMElements() {
        super.cacheDOMElements();

        this.$viewedMaster = this.$("#txt-viewed-master");
        this.directDownline = parseInt(<string>this.$("#directDownline").val());
        this.$errAgent = this.$("#msg-err-agent");
        this.usernameAutoSuggestionPath = this.$("#usernameAutoSuggestionPath").val();
        this.$output = this.$("#output");
    }
    viewAgents = (custId: number) => {
        this.$.blockUI();
        this.reportParam = {
            DownlineUsername: this.$("#txt-viewed-master").val(),
            Username: <string>this.$username.val(),
            SearchRoleId: siteInfrastructure.roleId.agent,
            CustId: custId,
            Status: <number>this.$dropdownStatus.find("option:selected").val(),
            PageSize: this.reportParam.PageSize,
            PageIndex: this.reportParam.PageIndex
        };

        if (this.directDownline != siteInfrastructure.roleId.agent &&
            (this.reportParam.DownlineUsername == "" ||
                this.reportParam.DownlineUsername == undefined)) {
            this.$errAgent.removeClass("display-none");
            this.$.unblockUI();
            return;
        }

        this.$errAgent.addClass("display-none");

        if (this.indexManager.isDevice) {
            this.indexManager.agentToggleHandler.hideFilter();
        }

        this.loadData();
    };
    renderAgentList = (rs: any) => {
        this.renderCustomerList(rs, 2, 'agent');
    };
    viewAgentsOfMaster = (custId: any, username: any, status: number) => {
        this.$viewedMaster.val(username);
        this.$username.val("");

        this.reportParam.PageIndex = 1;
        this.reportParam.PageSize = 100;
        this.reportParam.Status = status;

        this.statusComponent.changeSelection(status);

        this.viewAgents(custId);
    };
    reloadState = (reportParam: any) => {
        this.$("#txt-viewed-master").val(reportParam.DownlineUsername);
        this.baseReloadState(reportParam, 'Agent');
        this.viewAgents(reportParam.CustId);
    };
}