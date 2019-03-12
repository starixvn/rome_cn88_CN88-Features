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
        super($, accountingPeriodSelectionComponent, dateRangePickerComponent, sportGroupSelectionComponent)

        this.pagerComponent = pagerComponent;
        this.pagerComponent.pageList = this.pagerComponent.smallPageList;
    }

    init = () => {
        this.$.blockUI();
        this.cacheDOMElements();
        this.sportGroupSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-sportgroup", "UserSelectedSportGroupId"));
        this.accountingPeriodSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-period", "UserSelectedPeriodId", this.onPeriodChange));
        this.dateRangePickerComponent.render();

        this.initTooltip();
        this.pagerComponent.render();
        this.ptHistory = new PTHistory(this.$);
        this.ptHistory.init();
        this.registerEvents();

        this.$subMenuLegacy.parent().addClass("active");

        this.$.unblockUI();
    };

    public cacheDOMElements() {
        super.cacheDOMElements();
        this.$tooltip = this.$('[data-toggle="tooltip"]');
        this.$reportForm = this.$("#report-form");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$excelIcon = this.$("#icon-excel");
    }

    initTooltip = () => {
        this.$tooltip.tooltip();
    }

    public registerEvents() {
        this.$reportForm.submit(this.onReportSubmit);
        this.$excelIcon.click(this.onExportExcelEvent)
    }

    onReportSubmit = () => {
        this.sportGroupSelectionComponent.updateSelectedValue();
        this.accountingPeriodSelectionComponent.updateSelectedValue();
    };

    onExportExcelEvent = () => {
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    };
}