/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
"use strict";
class ReportIndexView {
    $: JQueryStatic;
    $wagerTab: JQuery;
    $cancelledTab: JQuery;
    $reportType: JQuery;
    $changeRptTypeLink: JQuery;
    $createRptForm: JQuery;

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
        this.dateRangePickerComponent.render();
        this.sportGroupSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-sportgroup", "UserSelectedSportGroupId"));
        this.betTypeSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-bettype", "UserSelectedBetTypeId"));
        this.betTypeSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-livestatus", "UserSelectedLiveStatusId"));
        this.registerEvents();
        this.$wagerTab.click();
        this.$.unblockUI();
    };

    cachedDOMElements = () => {
        this.$wagerTab = this.$("#wager-tab");
        this.$cancelledTab = this.$("#cancelled-tab");
        this.$reportType = this.$("#ReportType");
        this.$changeRptTypeLink = this.$(".changeRptType-link");
        this.$createRptForm = this.$("#create-rpt-form");
    };

    registerEvents = () => {
        var self = this;
        this.$wagerTab.on('click', (e) => {
            self.updateReportType(self.$(e.target).data("defaulttype"));
        });
        this.$cancelledTab.on('click', (e) => {
            self.updateReportType(self.$(e.target).data("defaulttype"));
        });
        this.$changeRptTypeLink.on('click touchend', (e) => {
            self.updateReportType(self.$(e.target).data("value"));
        });

        this.$createRptForm.submit(() => self.onSubmit());
    };

    public updateReportType(value) {
        this.$reportType.val(value);
    };

    public onSubmit() {
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