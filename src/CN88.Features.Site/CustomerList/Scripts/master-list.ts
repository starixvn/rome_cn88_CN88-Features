///// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
///// <reference path="../../shared/scripts/components/pager.ts" />
///// <reference path="base-list.ts" />

//class MasterList extends BaseList {
//    constructor(
//        $: JQueryStatic,
//        statusComponent: SingleSelectionDropdownComponent,
//        bootstrapDropdownComponent: BootstrapDropdownComponent,
//        numeral: any,
//        pager: PagerComponent) { 
//        super($, statusComponent, bootstrapDropdownComponent, numeral, pager, "Master");
//    };

//    public init(indexManager: any, agentList: BaseList) {
//        super.init(indexManager, siteInfrastructure.roleId.master, agentList);

//        super.cacheDOMElements();

//        this.$("#btn-submit-master").click(() => {
//            this.reportParam.PageIndex = 1;
//            this.viewList();
//        });
//    };

//    //reloadState(reportParam: any) {
//    //    this.baseReloadState(reportParam);
//    //    this.viewList();
//    //};
//}