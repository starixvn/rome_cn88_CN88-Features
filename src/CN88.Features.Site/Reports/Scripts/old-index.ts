/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
"use strict";
class OldReportIndexView extends OldBaseReport {
    $reportType: JQuery;
    $createRptForm: JQuery;
    $wagerRptTypeDropdown: JQuery;

    constructor(
        $: JQueryStatic,
        accountingPeriodSelectionComponent: SingleSelectionDropdownComponent,
        dateRangePickerComponent: DateRangePickerComponent,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent) {
        super($, accountingPeriodSelectionComponent, dateRangePickerComponent, sportGroupSelectionComponent);
    }

    init = () => {
        this.$.blockUI();

        this.cachedDOMElements();
        this.renderRptTypeDropdown();
        this.accountingPeriodSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-period", "UserSelectedPeriodId"));
        this.dateRangePickerComponent.render();
        this.sportGroupSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-sportgroup", "UserSelectedSportGroupId"));
        this.registerEvents();

        this.$.unblockUI();
    }

    cachedDOMElements = () => {
        this.$reportType = this.$("#ReportType");
        this.$createRptForm = this.$("#create-rpt-form");
        this.$wagerRptTypeDropdown = this.$("#dropdown-wagerRptType");
    };

    renderRptTypeDropdown = () => {
        this.$wagerRptTypeDropdown.multiselect({
            multiple: false,
            maxHeight: 300,
            numberDisplayed: 1,
            includeSelectAllOption: false,
        });
    };

    registerEvents = () => {
        this.$createRptForm.submit(this.onSubmit);
    }

    onSubmit = () => {
        let reportType = this.$wagerRptTypeDropdown.children("option:selected").val();
        this.$reportType.val(reportType);
        this.accountingPeriodSelectionComponent.updateSelectedValue();
        this.sportGroupSelectionComponent.updateSelectedValue();
    }
}

$(function () {
    const oldReportIndexView = new OldReportIndexView(
        $,
        new SingleSelectionDropdownComponent($),
        new DateRangePickerComponent($),
        new SingleSelectionDropdownComponent($));

    oldReportIndexView.init();
})