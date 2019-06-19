/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="../../shared/scripts/components/pager.ts" />
/// <reference path="base-list.ts" />

class MasterList extends BaseList {
    searchSuggestionMaster: JQuery;
    usernameAutoSuggestionPath: string;
    $output: JQuery;

    constructor(
        $: JQueryStatic,
        statusComponent: SingleSelectionDropdownComponent,
        bootstrapDropdownComponent: BootstrapDropdownComponent,
        numeral: any,
        pager: PagerComponent) {
        super($, statusComponent, bootstrapDropdownComponent, numeral, pager, "Master");
    }

    public init(indexManager: any) {
        super.init(
            indexManager,
            siteInfrastructure.roleId.master,
            indexManager.viewMaster);

        this.cacheDOM();

        this.$("#btn-submit-master").click(() => {
            this.reportParam.PageIndex = 1;
            this.viewMasters();
        });
    }

    cacheDOM() {
        this.searchSuggestionMaster = this.$("#txt-username-master");
        this.usernameAutoSuggestionPath = this.$("#usernameAutoSuggestionPath").val();
        this.$output = this.$("#output");
    };
    viewMasters() {
        this.viewList();
    };
    reloadState(reportParam: any) {
        this.baseReloadState(reportParam, 'Master');
        this.viewMasters();
    };
}