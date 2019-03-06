/// <reference path="../../shared/scripts/core.ts" />
/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
/// <reference path="../../shared/scripts/components/pager.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
"use strict";
class WagerReport extends LegacyReport {
    $reportType: JQuery;
    $tabValidStake: JQuery;

    elements = {
        tabSelectors: null,
        tabContent: null
    };

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        betTypeSelectionComponent: SingleSelectionDropdownComponent,
        liveStatusSelectionComponent: SingleSelectionDropdownComponent,
        pagerComponent: PagerComponent) {
        super($, dateRangePickerComponent, sportGroupSelectionComponent, betTypeSelectionComponent, liveStatusSelectionComponent, pagerComponent)
    }

    init = () => {
        this.$.blockUI();
        this.cacheDOMElements();
        this.dateRangePickerComponent.render();
        this.sportGroupSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-sportgroup", "UserSelectedSportGroupId"));
        this.betTypeSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-bettype", "UserSelectedBetTypeId"));
        this.liveStatusSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-livestatus", "UserSelectedLiveStatusId"));
        this.pagerComponent.pageList = this.pagerComponent.pageList;
        this.initTooltip();
        this.pagerComponent.render();
        this.registerEvents();

        if (this.$reportType.val() == "2") {
            this.$tabValidStake.click();
        }

        this.$.unblockUI();
    };

    public cacheDOMElements() {
        super.cacheDOMElements();
        this.$reportType = this.$("#ReportType");
        this.$tabValidStake = this.$("#tabValidStake");
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
    const wagerReport = new WagerReport(
        $,
        new DateRangePickerComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new PagerComponent($));

    wagerReport.init();
})