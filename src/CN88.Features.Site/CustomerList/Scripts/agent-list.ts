///// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
///// <reference path="../../shared/scripts/components/pager.ts" />
///// <reference path="base-list.ts" />

//declare var initAutoComplete: Function;

//class AgentList extends BaseList {
//    $viewedMaster: JQuery;

//    constructor(
//        $: JQueryStatic,
//        statusComponent: SingleSelectionDropdownComponent,
//        bootstrapDropdownComponent: BootstrapDropdownComponent,
//        numeral: any,
//        pager: PagerComponent) {
//        super($, statusComponent, bootstrapDropdownComponent, numeral, pager, "Agent");
//    };

//    public init(indexManager: any, memberList: BaseList) {
//        super.init(indexManager, siteInfrastructure.roleId.agent, memberList);

//        this.cacheDOMElements();

//        this.$("#btn-submit-agent").off("click").on("click", () => {
//            this.reportParam.PageIndex = 1;
//            this.viewList(this.$viewedDirectUpline.val(), this.reportParam.CustId);
//        });
//    };

//    public cacheDOMElements() {
//        super.cacheDOMElements();
//        this.$viewedDirectUpline = this.$("#txt-viewed-master");
//        this.$viewedMaster = this.$("#txt-viewed-master");
//    };

//    //viewAgents(custId: number) {
//    //    this.viewList(this.$viewedDirectUpline.val(), custId);
//    //    const directDownline = parseInt(<string>this.$("#directDownline").val()),
//    //        $errAgent = this.$("#msg-err-agent"),
//    //        { DownlineUsername } = this.reportParam;

//    //    if (directDownline != siteInfrastructure.roleId.agent &&
//    //        (DownlineUsername == "" || DownlineUsername == undefined)) {
//    //        $errAgent.removeClass("display-none");
//    //        this.$.unblockUI();
//    //        return;
//    //    }

//    //    $errAgent.addClass("display-none");
//    //};

//    //viewDownlines(custId: number, username: string, status: number) {
//    //    this.$viewedDirectUpline.val(username);
//    //    this.setStateWhenViewDownlines(status);
//    //    this.viewList(username, custId);
//    //    //this.viewAgents(custId);
//    //};

//    //reloadState(reportParam: any) {
//    //    const { DownlineUsername } = reportParam;
//    //    this.$viewedDirectUpline.val(DownlineUsername);
//    //    this.baseReloadState(reportParam);
//    //    this.viewList(DownlineUsername, reportParam.CustId);
//    //    //this.viewAgents(reportParam.CustId);
//    //};
//}