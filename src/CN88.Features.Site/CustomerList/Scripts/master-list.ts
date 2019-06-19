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
    }

    cacheDOM = () => {
        this.searchSuggestionMaster = this.$("#txt-username-master");
        this.usernameAutoSuggestionPath = this.$("#usernameAutoSuggestionPath").val();
        this.$output = this.$("#output");
    };

    renderMasterList = (rs: any) => {
        if (rs.SearchAccountId > 0) {
            var buttonAddNew = this.$(".add-directdownline-master");
            buttonAddNew.attr("cust-id", rs.SearchAccountId);
            buttonAddNew.attr("username", rs.Username);
            buttonAddNew.attr("downline-username", rs.DownlineUsername);
        }

        if (!rs.Customers) {
            return;
        }

        this.$tblReport.removeClass("display-none");

        this.$tblReport.find("> tbody").empty().html(
            this.indexManager.isDevice ? this.buildBodyOnMobileMode(rs, 3) : this.buildBodyOnDesktopMode(rs, 3)
        );
        this.$(window).trigger('resize.stickyTableHeaders');
        super.bindReportEvents(rs);
    };
    reloadState = (reportParam: any) => {
        this.$username.val(reportParam.Username);
        this.$dropdownStatus.val(this.$("#dropdown-status-master").val());
        this.reportParam.PageSize = reportParam.PageSize;
        this.reportParam.PageIndex = reportParam.PageIndex;

        this.$("#PageIndexMaster").val(reportParam.PageIndex);
        this.$("#PageSizeMaster").val(reportParam.PageIndex);

        this.viewList();
    };
    initAutoComplete = () => {
        let queryUsernameURL = this.usernameAutoSuggestionPath + "?" +
            this.$.param({ isDir: true, issub: false, ismember: false });
        //initAutoComplete(this.searchSuggestionMaster[0], queryUsernameURL);
        this.$output.css("top", this.searchSuggestionMaster.offset().top + 30);
        this.$output.css("left", this.searchSuggestionMaster.offset().left);
    };
}