/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
"use strict";
class OldReportIndexView extends OldBaseReport {
    $displayMode: JQuery;
    $reportType: JQuery;
    $createRptForm: JQuery;
    $wagerRptTypeDropdown: JQuery;
    $clearBtn: JQuery;
    $fromDate: number;
    $toDate: number;

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

        let displayMode = this.$displayMode.val();
        this.$subMenuItems.removeClass("active");
        if (displayMode === "1") {
            this.$subMenuWager.parent().addClass("active");
        } else {
            this.$subMenuLegacy.parent().addClass("active");
        }

        this.$.unblockUI();
    }

    cachedDOMElements = () => {
        super.cacheDOMElements();
        this.$displayMode = this.$("#DisplayMode");
        this.$reportType = this.$("#ReportType");
        this.$createRptForm = this.$("#create-rpt-form");
        this.$wagerRptTypeDropdown = this.$("#dropdown-wagerRptType");
        this.$clearBtn = this.$("#btn-clear");

        this.$fromDate = this.$("#from-date").val();
        this.$toDate = this.$("#to-date").val();
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
        this.reset();
    }

    onSubmit = () => {
        let reportType = this.$wagerRptTypeDropdown.children("option:selected").val();
        this.$reportType.val(reportType);
        this.accountingPeriodSelectionComponent.updateSelectedValue();
        this.sportGroupSelectionComponent.updateSelectedValue();
    }

    reset = () => {
        this.$clearBtn.click(() => {
            this.resetReportType();
            this.resetAccountingSheet();
            this.resetDatePicker();
            this.resetSportType();
        });
    }

    resetReportType = () => {
        (<HTMLSelectElement>this.$wagerRptTypeDropdown[0]).selectedIndex = 0;
        this.$wagerRptTypeDropdown.multiselect("refresh");
    }

    resetAccountingSheet = () => {
        (<HTMLSelectElement>this.accountingPeriodSelectionComponent.$dropdown[0]).selectedIndex = 0;
        this.accountingPeriodSelectionComponent.$dropdown.multiselect("refresh");
    }

    resetDatePicker = () => {
        this.$("#from-date").val(this.$fromDate);
        this.$("#to-date").val(this.$toDate);
    }

    resetSportType = () => {
        (<HTMLSelectElement>this.sportGroupSelectionComponent.$dropdown[0]).selectedIndex = 0;
        this.sportGroupSelectionComponent.$dropdown.multiselect("refresh");
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