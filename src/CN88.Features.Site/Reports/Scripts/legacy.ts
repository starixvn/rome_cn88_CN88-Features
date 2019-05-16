/// <reference path="../../shared/scripts/core.ts" />
/// <reference path="pt-history.ts" />
/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="../../shared/scripts/components/sort.ts" />
/// <reference path="../../shared/scripts/components/link-builder.ts" />
/// <reference path="../../shared/scripts/components/client-pager.ts" />
"use strict";
class LegacyReport extends BaseReport {
    $tooltip: JQuery;
    $reportForm: JQuery;
    $exportExcelForm: JQuery;
    $excelIcon: JQuery;
    $tableReport: JQuery;
    $custLevelId: JQuery;

    sortComponent: SortComponent;
    linkBuilderComponent: LinkBuilderComponent;
    pagerComponent: ClientPagerComponent;
    //ptHistory: PTHistory;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        betTypeSelectionComponent: SingleSelectionDropdownComponent,
        liveStatusSelectionComponent: SingleSelectionDropdownComponent,
        sortComponent: SortComponent,
        linkBuilderComponent: LinkBuilderComponent,
        pagerComponent: ClientPagerComponent) {
        super($, dateRangePickerComponent, sportGroupSelectionComponent, betTypeSelectionComponent, liveStatusSelectionComponent);

        this.sortComponent = sortComponent;
        this.linkBuilderComponent = linkBuilderComponent;
        this.pagerComponent = pagerComponent;
        this.pagerComponent.pageList = this.pagerComponent.smallPageList;
    }

    init = () => {
        this.$.blockUI();

        this.cacheDOMElements();
        this.renderComponents();
        //this.ptHistory = new PTHistory(this.$);
        //this.ptHistory.init();
        this.registerEvents();

        this.$.unblockUI();
    };

    renderComponents(): void {
        this.initTooltip();
        super.renderComponents();

        let headers = this.$custLevelId.val() == "4"
            ? { 0: { sorter: "text" } }
            : { 0: { sorter: "text" }, 1: { sorter: "text" } };
        this.sortComponent.render(
            new SortOption(
                this.$tableReport,
                parseInt(this.$tableReport.data("totalrecords"), 0) > 1,
                true,
                this.sortComponent.afterSort,
                headers,
                this.$reportForm));

        this.linkBuilderComponent.render();
        this.pagerComponent.render(new ClientPagerOption(() => { this.onSelectPage(); }));
    }

    initTooltip = () => {
        this.$tooltip.tooltip();
    }

    public cacheDOMElements() {
        this.$tooltip = this.$('[data-toggle="tooltip"]');
        this.$reportForm = this.$("#report-form");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$excelIcon = this.$("#icon-excel");
        this.$tableReport = this.$("#tbl-report");
        this.$custLevelId = this.$("#CustLevelId");
    }

    public registerEvents() {
        this.$reportForm.submit(this.onReportSubmit);
        this.$excelIcon.click(this.onExportExcelEvent)
    }

    onSelectPage(): void {
        this.linkBuilderComponent.updateParam("PageSize", this.pagerComponent.pageSize.toString());
    }

    onReportSubmit = () => {
        this.sportGroupSelectionComponent.updateSelectedValue();
        this.betTypeSelectionComponent.updateSelectedValue();
        this.liveStatusSelectionComponent.updateSelectedValue();
    };

    onExportExcelEvent = () => {
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    };
}