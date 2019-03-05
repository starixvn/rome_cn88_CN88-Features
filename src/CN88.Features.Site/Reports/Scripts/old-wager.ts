/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
/// <reference path="../../shared/scripts/components/pager.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
"use strict";
class OldWagerReport extends OldLegacyReport {
    elements = {
        tabSelectors: null,
        tabContent: null
    };

    constructor(
        $: JQueryStatic,
        accountingPeriodSelectionComponent: SingleSelectionDropdownComponent,
        dateRangePickerComponent: DateRangePickerComponent,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        pagerComponent: PagerComponent) {
        super($, accountingPeriodSelectionComponent, dateRangePickerComponent, sportGroupSelectionComponent, pagerComponent)
    }

    init = () => {
        this.$.blockUI();
        this.cacheDOMElements();
        this.sportGroupSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-sportgroup", "UserSelectedSportGroupId"));
        this.accountingPeriodSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-period", "UserSelectedPeriodId"));
        this.dateRangePickerComponent.render();
        this.pagerComponent.pageList = this.pagerComponent.pageList;
        this.initTooltip();
        this.pagerComponent.render();
        this.registerEvents();
        this.$.unblockUI();
    };

    public cacheDOMElements() {
        super.cacheDOMElements();
        this.elements.tabSelectors = $(".tab-selector");
        this.elements.tabContent = $(".tab-content");
    }

    public registerEvents() {
        let self = this;
        super.registerEvents();

        this.elements.tabSelectors.on("click touchend", function () {
            self.elements.tabSelectors.removeClass("active");
            $(this).addClass("active");
            var target = $(this).attr("target-id");
            self.elements.tabContent.addClass("display-none");
            $("#" + target).removeClass("display-none");
        });
    }
}

$(function () {
    const oldWagerReport = new OldWagerReport(
        $,
        new SingleSelectionDropdownComponent($),
        new DateRangePickerComponent($),
        new SingleSelectionDropdownComponent($),
        new PagerComponent($));

    oldWagerReport.init();
})