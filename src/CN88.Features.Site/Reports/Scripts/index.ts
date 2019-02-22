/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
"use strict";
class ReportIndexView {
    $: JQueryStatic;
    $wagerTab: JQuery;
    $cancelledTab: JQuery;
    $reportType: JQuery;
    $createRptForm: JQuery;
    $wagerRptTypeDropdown: JQuery;
    $cancelledRptTypeDropdown: JQuery;

    dateRangePickerComponent: DateRangePickerComponent;
    sportGroupSelectionComponent: SingleSelectionDropdownComponent;
    betTypeSelectionComponent: SingleSelectionDropdownComponent;
    liveStatusSelectionComponent: SingleSelectionDropdownComponent;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        betTypeSelectionComponent: SingleSelectionDropdownComponent,
        liveStatusSelectionComponent: SingleSelectionDropdownComponent) {
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.sportGroupSelectionComponent = sportGroupSelectionComponent;
        this.betTypeSelectionComponent = betTypeSelectionComponent;
        this.liveStatusSelectionComponent = liveStatusSelectionComponent;
    }

    init = () => {
        this.$.blockUI();

        this.cachedDOMElements();
        this.renderRptTypeDropdown();
        this.dateRangePickerComponent.render();
        this.sportGroupSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-sportgroup", "UserSelectedSportGroupId"));
        this.betTypeSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-bettype", "UserSelectedBetTypeId"));
        this.betTypeSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-livestatus", "UserSelectedLiveStatusId"));
        this.registerEvents();

        this.$.unblockUI();
    };

    cachedDOMElements = () => {
        this.$wagerTab = this.$("#wager-tab");
        this.$cancelledTab = this.$("#cancelled-tab");
        this.$reportType = this.$("#ReportType");
        this.$createRptForm = this.$("#create-rpt-form");
        this.$wagerRptTypeDropdown = this.$("#dropdown-wagerRptType");
        this.$cancelledRptTypeDropdown = this.$("#dropdown-cancelledRptType");
    };

    renderRptTypeDropdown = () => {
        this.$wagerRptTypeDropdown.multiselect({
            multiple: false,
            maxHeight: 300,
            numberDisplayed: 1,
            includeSelectAllOption: false,
        });

        this.$cancelledRptTypeDropdown.multiselect({
            multiple: false,
            maxHeight: 300,
            numberDisplayed: 1,
            includeSelectAllOption: false,
        });
    };

    registerEvents = () => {
        var self = this;

        this.$createRptForm.submit(() => self.onSubmit());
    };

    updateReportTypeValue = () => {
        let selected = [];
        let selectedOptions;
        let this$ = this.$;

        if (this.$wagerTab.hasClass("active")) {
            selectedOptions = this.$wagerRptTypeDropdown.find("option:selected");

            selectedOptions.each(function () {
                selected.push([this$(this).val()]);
            });
        }

        if (this.$cancelledTab.hasClass("active")) {
            selectedOptions = this.$cancelledRptTypeDropdown.find("option:selected");

            selectedOptions.each(function () {
                selected.push([this$(this).val()]);
            });
        }

        this.$reportType.val(selected[0]);
    };

    public onSubmit() {
        this.updateReportTypeValue();
        this.sportGroupSelectionComponent.updateSelectedValue();
        this.betTypeSelectionComponent.updateSelectedValue();
        this.liveStatusSelectionComponent.updateSelectedValue();
    }
}
$(function () {
    const reportIndexView = new ReportIndexView(
        $,
        new DateRangePickerComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($));

    reportIndexView.init();
})