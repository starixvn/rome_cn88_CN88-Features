/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="../../shared/scripts/components/sort.ts" />
/// <reference path="../../shared/scripts/components/link-builder.ts" />
/// <reference path="../../shared/scripts/components/filter-toggle.ts" />
/// <reference path="../../shared/scripts/components/client-pager.ts" />
/// <reference path="legacy.ts" />

class WagerReport extends LegacyReport {
    $tabValidStake: JQuery;
    $tabSettlement: JQuery;
    $tableContent: JQuery;
    $isSettlement: JQuery;
    isOwner: boolean;
    $tabMainContent: JQuery;
    toggleHandler: ToggleHandler;
    $nowDataRow: JQuery;
    isCancelledReport: boolean;

    totalRecords: number;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        betTypeSelectionComponent: SingleSelectionDropdownComponent,
        liveStatusSelectionComponent: SingleSelectionDropdownComponent,
        sortComponent: SortComponent,
        linkBuilderComponent: LinkBuilderComponent,
        pagerComponent: ClientPagerComponent,
        toggleHandler: ToggleHandler) {
        super(
            $,
            dateRangePickerComponent,
            sportGroupSelectionComponent,
            betTypeSelectionComponent,
            liveStatusSelectionComponent,
            sortComponent,
            linkBuilderComponent,
            pagerComponent);

        this.toggleHandler = toggleHandler;
    }

    init = () => {
        this.$.blockUI();

        this.initForMobile();
        this.cacheDOMElements();
        this.renderComponents();
        this.registerEvents();

        this.$.unblockUI();
    };

    initForMobile = () => {
        const reportTable = this.$("#tbl-report");
        this.isMobile = cn88Features.convertAttributeToBool(reportTable.attr("data-isMobile"));
        this.isCancelledReport = cn88Features.convertAttributeToBool(reportTable.attr("data-cancelledreport"));
        this.isOwner = cn88Features.convertAttributeToBool(this.$("#IsOwner").val());
        if (this.isMobile) {
            if (!this.isCancelledReport) {
                reportTable.stacktable();
            }
            this.$('body').backtop();
        }
    };

    cacheDOMElements(): void {
        super.cacheDOMElements();

        this.$tabValidStake = this.$("#tabValidStake");
        this.$tabSettlement = this.$("#tabSettlement");
        this.$isSettlement = this.$("#IsSettlement");
        this.$tabMainContent = this.$("#tab-main-content,#report-pager");
        this.$nowDataRow = this.$("#no-data-row");
        this.totalRecords = this.$tableReport.data("totalrecords");
    }

    registerEvents(): void {
        if (this.isMobile) {
            this.$tableReport.find("th").removeClass("sorting");
            this.toggleHandler.render(new ToggleHandlerOption("filter-toggle-icon", "report-form", "tbl-report-stack,#tbl-report"));
        }
        else {
            this.$tabSettlement.on("click touchend", this.onTabSettlementClickEvent);

            this.$tabValidStake.on("click touchend", this.onTabValidStakeClickEvent);
        }

        super.registerEvents();

        if (this.isMobile && this.isOwner) {
            this.$(".btn-collapse-expanse").trigger("click");
        }
    }

    onTabSettlementClickEvent = () => {
        if (this.$custLevelId.val() !== "1") {
            this.$isSettlement.val("True");
            this.linkBuilderComponent.updateParam("IsSettlement", "True");
        }

        this.$(".tab-selector").removeClass("active");
        this.$tabSettlement.addClass("active");
        this.$tableReport.addClass("settlement-report");

        if (this.totalRecords > 0 && this.$custLevelId.val() === "4") {
            this.$("#profit-margin").attr("colspan", this.$tableReport.data("numberofsettlementcolumns"));
        }

        if (this.totalRecords < 1) {
            this.$nowDataRow.attr("colspan", this.$tableReport.data("numberofsettlementcolumns"));
        }
    };

    onTabValidStakeClickEvent = () => {
        this.$isSettlement.val("False");
        this.linkBuilderComponent.updateParam("IsSettlement", "False");

        this.$(".tab-selector").removeClass("active");
        this.$tabValidStake.addClass("active");
        this.$tableReport.removeClass("settlement-report");

        if (this.totalRecords > 0 && this.$custLevelId.val() === "4") {
            this.$("#profit-margin").attr("colspan", this.$tableReport.data("numberofvalidstakecolumns"));
        }

        if (this.totalRecords < 1) {
            this.$nowDataRow.attr("colspan", this.$tableReport.data("numberofvalidstakecolumns"));
        }
    };
}

$(function () {
    const linkBuilderComponent = new LinkBuilderComponent($);
    const wagerReport = new WagerReport(
        $,
        new DateRangePickerComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new SortComponent($, linkBuilderComponent),
        linkBuilderComponent,
        new ClientPagerComponent($),
        new ToggleHandler($));

    wagerReport.init();
});