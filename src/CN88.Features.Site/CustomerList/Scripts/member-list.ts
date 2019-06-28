///// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
///// <reference path="base-list.ts" />
///// <reference path="../../shared/scripts/components/pager.ts" />

//class MemberList extends BaseList {
//    $viewedAgent: JQuery;

//    constructor(
//        $: JQueryStatic,
//        statusComponent: SingleSelectionDropdownComponent,
//        bootstrapDropdownComponent: BootstrapDropdownComponent,
//        numeral: any,
//        pager: PagerComponent) {
//        super($, statusComponent, bootstrapDropdownComponent, numeral, pager, "Member");
//    }

//    public init(indexManager: any) {
//        super.init(indexManager, siteInfrastructure.roleId.member, null);

//        this.cacheDOMElements();

//        this.$("#btn-submit-member").off("click").on("click", () => {
//            this.reportParam.PageIndex = 1;
//            this.viewList(this.$viewedDirectUpline.val(), this.reportParam.CustId);
//        });
//    };

//    public cacheDOMElements() {
//        super.cacheDOMElements();
//        this.$viewedDirectUpline = this.$("#txt-viewed-agent");
//        this.$viewedAgent = this.$("#txt-viewed-agent");
//    };

//    //viewMembers(custId: number) {
//    //    this.viewList(this.$viewedDirectUpline.val(), custId);
//    //    const directDownline = parseInt(this.$("#directDownline").val()),
//    //        $errMember = this.$("#msg-err-member"),
//    //        { DownlineUsername } = this.reportParam;

//    //    if (directDownline != siteInfrastructure.roleId.member &&
//    //        (DownlineUsername == "" || DownlineUsername == undefined)) {
//    //        $errMember.removeClass("display-none");
//    //        this.$.unblockUI();
//    //        return;
//    //    }

//    //    $errMember.addClass("display-none");
//    //};

//    //viewDownlines(custId: number, username: string, status: number) {
//    //    this.$viewedDirectUpline.val(username);
//    //    this.setStateWhenViewDownlines(status);
//    //    this.viewList(username, custId);
//    //};

//    //reloadState(reportParam: any) {
//    //    const { DownlineUsername } = reportParam;
//    //    this.$viewedDirectUpline.val(DownlineUsername);
//    //    this.baseReloadState(reportParam);
//    //    this.viewList(DownlineUsername, reportParam.CustId);
//    //};
//}