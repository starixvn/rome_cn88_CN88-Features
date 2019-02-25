/// <reference path="../../shared/scripts/core.ts" />
/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
/// <reference path="../../shared/scripts/components/pager.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
"use strict";
class LegacyReport extends BaseReport {
    $reportForm: JQuery;

    pagerComponent: PagerComponent;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        betTypeSelectionComponent: SingleSelectionDropdownComponent,
        liveStatusSelectionComponent: SingleSelectionDropdownComponent,
        pagerComponent: PagerComponent) {
        super($, dateRangePickerComponent, sportGroupSelectionComponent, betTypeSelectionComponent, liveStatusSelectionComponent)

        this.pagerComponent = pagerComponent;
    }

    init = () => {
        this.$.blockUI();
        this.cacheDOMElements();

        this.dateRangePickerComponent.render();
        this.sportGroupSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-sportgroup", "UserSelectedSportGroupId"));
        this.betTypeSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-bettype", "UserSelectedBetTypeId"));
        this.liveStatusSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-livestatus", "UserSelectedLiveStatusId"));
        this.pagerComponent.pageList = this.pagerComponent.pageList;
        this.pagerComponent.render();

        this.registerEvents();
        this.$.unblockUI();
    };

    public cacheDOMElements() {
        this.$reportForm = this.$("#report-form");
    }

    public registerEvents() {
        let self = this;
        this.$reportForm.submit(() => self.onReportSubmit());
    }

    public onReportSubmit() {
        this.sportGroupSelectionComponent.updateSelectedValue();
        this.betTypeSelectionComponent.updateSelectedValue();
        this.liveStatusSelectionComponent.updateSelectedValue();
    }
}

$(function () {
    const legacyReport = new LegacyReport(
        $,
        new DateRangePickerComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new PagerComponent($));

    legacyReport.init();
})