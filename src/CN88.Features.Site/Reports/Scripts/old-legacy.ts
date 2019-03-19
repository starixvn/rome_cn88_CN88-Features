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

    pagerComponent: ClientPagerComponent;
    ptHistory: PTHistory;

    constructor(
        $: JQueryStatic,
        accountingPeriodSelectionComponent: SingleSelectionDropdownComponent,
        dateRangePickerComponent: DateRangePickerComponent,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        pagerComponent: ClientPagerComponent) {
        super($, accountingPeriodSelectionComponent, dateRangePickerComponent, sportGroupSelectionComponent);

        this.pagerComponent = pagerComponent;
        this.pagerComponent.pageList = this.pagerComponent.smallPageList;
    }

    init(): void {
        this.$.blockUI();

        this.reCheckIn();
        this.cacheDOMElements();
        this.renderComponents();

        this.ptHistory = new PTHistory(this.$);
        this.ptHistory.init();

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
    }

    renderComponents(): void {
        this.initTooltip();
        super.renderComponents();
        this.pagerComponent.render();
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
}