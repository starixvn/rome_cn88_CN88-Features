/// <reference path="../../shared/scripts/components/client-pager.ts" />
/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
"use strict";
class OldWagerReport extends OldLegacyReport {
    $tabSettlement: JQuery;

    elements = {
        tabSelectors: null,
        tabContent: null
    };

    constructor(
        $: JQueryStatic,
        accountingPeriodSelectionComponent: SingleSelectionDropdownComponent,
        dateRangePickerComponent: DateRangePickerComponent,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        pagerComponent: ClientPagerComponent) {
        super($, accountingPeriodSelectionComponent, dateRangePickerComponent, sportGroupSelectionComponent, pagerComponent)
    }

    init = () => {
        this.$.blockUI();
        this.cacheDOMElements();
        this.sportGroupSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-sportgroup", "UserSelectedSportGroupId"));
        this.accountingPeriodSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-period", "UserSelectedPeriodId"));
        this.dateRangePickerComponent.render();
        this.initTooltip();
        this.registerEvents();
        this.$tabSettlement.click();

        this.$subMenuWager.parent().addClass("active");
        this.$.unblockUI();
    };

    public cacheDOMElements() {
        super.cacheDOMElements();
        this.$tabSettlement = this.$("#tabSettlement");
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
            var pagerOption = new ClientPagerOption(null, "report-pager", "report-form", "PageIndex", "PageSize", target + " > .tbl-report");
            self.pagerComponent.render(pagerOption);
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
        new ClientPagerComponent($));

    oldWagerReport.init();
})