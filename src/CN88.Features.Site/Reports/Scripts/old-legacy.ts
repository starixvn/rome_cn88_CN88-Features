/// <reference path="../../shared/scripts/components/client-pager.ts" />
/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
/// <reference path="pt-history.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
"use strict";
class OldLegacyReport extends OldBaseReport {
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
        accountingPeriodSelectionComponent: SingleSelectionDropdownComponent,
        dateRangePickerComponent: DateRangePickerComponent,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        sortComponent: SortComponent,
        linkBuilderComponent: LinkBuilderComponent,
        pagerComponent: ClientPagerComponent) {
        super($, accountingPeriodSelectionComponent, dateRangePickerComponent, sportGroupSelectionComponent);

        this.sortComponent = sortComponent;
        this.linkBuilderComponent = linkBuilderComponent;
        this.pagerComponent = pagerComponent;
        this.pagerComponent.pageList = this.pagerComponent.smallPageList;
    }

    init(): void {
        this.$.blockUI();

        this.reCheckIn();
        this.cacheDOMElements();
        this.renderComponents();

        //this.ptHistory = new PTHistory(this.$);
        //this.ptHistory.init();

        this.registerEvents();

        this.$subMenuLegacy.parent().addClass("active");

        this.$.unblockUI();
    }

    cacheDOMElements(): void {
        super.cacheDOMElements();
        this.$tooltip = this.$('[data-toggle="tooltip"]');
        this.$reportForm = this.$("#report-form");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$excelIcon = this.$("#icon-excel");
        this.$tableReport = this.$("#tbl-report");
        this.$custLevelId = this.$("#CustLevelId");
    }

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

    initTooltip(): void {
        this.$tooltip.tooltip();
    }

    registerEvents(): void {
        this.$reportForm.submit(() => { this.onReportSubmit(); });
        this.$excelIcon.click(() => { this.onExportExcelEvent(); })
    }

    onReportSubmit(): void {
        this.sportGroupSelectionComponent.updateSelectedValue();
        this.accountingPeriodSelectionComponent.updateSelectedValue();
    }

    onExportExcelEvent(): void {
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    };

    onSelectPage(): void {
        this.linkBuilderComponent.updateParam("PageSize", this.pagerComponent.pageSize.toString());
    }
}