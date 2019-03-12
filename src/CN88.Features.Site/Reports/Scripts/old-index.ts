/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
"use strict";
class OldReportIndexView extends OldBaseReport {
    $displayMode: JQuery;
    $reportType: JQuery;
    $createRptForm: JQuery;
    $wagerRptTypeDropdown: JQuery;
    expiredSessionSeconds: number;

    constructor(
        $: JQueryStatic,
        accountingPeriodSelectionComponent: SingleSelectionDropdownComponent,
        dateRangePickerComponent: DateRangePickerComponent,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent) {
        super($, accountingPeriodSelectionComponent, dateRangePickerComponent, sportGroupSelectionComponent);
    }

    init = () => {
        this.$.blockUI();
        this.expiredSessionSeconds = this.$('#expiredSessionSeconds').val();
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
        this.checkIn();
    }

    onSubmit = () => {
        let reportType = this.$wagerRptTypeDropdown.children("option:selected").val();
        this.$reportType.val(reportType);
        this.accountingPeriodSelectionComponent.updateSelectedValue();
        this.sportGroupSelectionComponent.updateSelectedValue();
    }

    checkIn = () => {
        setInterval(function () {
            this.$.ajax({ url: '../OldReports/Checkin', type: 'POST' });
        }, this.expiredSessionSeconds * 1000);
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