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

    public init(viewResources: any, indexManager: any) {
        super.init(
            viewResources,
            indexManager,
            this.renderMasterList,
            siteInfrastructure.roleId.master,
            indexManager.viewMaster,
            indexManager.addNewDownline);

        this.cacheDOM();

        var self = this;
        this.$("#btn-submit-master").click(function () {
            self.reportParam.PageIndex = 1;
            self.viewMasters();
        });
    }

    cacheDOM = () => {
        this.searchSuggestionMaster = this.$("#txt-username-master");
        this.usernameAutoSuggestionPath = this.$("#usernameAutoSuggestionPath").val();
        this.$output = this.$("#output");
    };
    viewMasters = () => {
        this.$.blockUI();
        this.reportParam = {
            DownlineUsername: null,
            Username: <string>this.$username.val(),
            SearchRoleId: this.levelId,
            CustId: 0,
            Status: <number>this.$dropdownStatus.find("option:selected").val(),
            PageSize: this.reportParam.PageSize,
            PageIndex: this.reportParam.PageIndex
        };

        if (this.indexManager.isDevice) {
            this.indexManager.masterToggleHandler.hideFilter();
        }

        this.loadData();
    };
    renderMasterList = (rs: any) => {
        this.renderCustomerList(rs, 3, 'master');
    };
    reloadState = (reportParam: any) => {
        this.baseReloadState(reportParam, 'Master');
        this.viewMasters();
    };
}