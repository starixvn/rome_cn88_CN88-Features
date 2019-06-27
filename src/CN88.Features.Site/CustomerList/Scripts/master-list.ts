/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="../../shared/scripts/components/pager.ts" />
/// <reference path="base-list.ts" />

class MasterList extends BaseList {
    constructor(
        $: JQueryStatic,
        statusComponent: SingleSelectionDropdownComponent,
        bootstrapDropdownComponent: BootstrapDropdownComponent,
        numeral: any,
        pager: PagerComponent) {
        super($, statusComponent, bootstrapDropdownComponent, numeral, pager, "Master");
    };

    public init(indexManager: any) {
        super.init(
            indexManager,
            siteInfrastructure.roleId.master,
            indexManager.viewMaster);

        super.cacheDOMElements();

        this.$("#btn-submit-master").click(() => {
            this.reportParam.PageIndex = 1;
            this.viewMasters();
        });
    };

    viewMasters() {
        this.viewList();
    };

    reloadState(reportParam: any) {
        this.baseReloadState(reportParam);
        this.viewMasters();
    };
}