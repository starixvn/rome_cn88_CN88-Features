/// <reference path="../../shared/scripts/components/client-pager.ts" />
/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
"use strict";
class OldWagerReport extends OldLegacyReport {
    $tabSettlement: JQuery;
    $tabValidStake: JQuery;

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
        this.accountingPeriodSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-period", "UserSelectedPeriodId", this.onPeriodChange));
        this.dateRangePickerComponent.render();
        this.initTooltip();
        this.registerEvents();

        this.$subMenuWager.parent().addClass("active");

        if (location.search.indexOf("IsSettlement=True") != -1 ||
            location.search.indexOf("CustLevelId=1") != -1 ||
            this.$tabSettlement.hasClass("active")) {
            this.activateSettlementTab();
        } else {
            this.activateValidStakeTab();
        }

        this.$.unblockUI();
    };

    activateSettlementTab = () => {
        this.$tabSettlement
            .removeClass("active")
            .addClass("active");

        this.$tabValidStake
            .removeClass("active");

        this.$tabSettlement.click();
    };

    activateValidStakeTab = () => {
        this.$tabValidStake
            .removeClass("active")
            .addClass("active");

        this.$tabSettlement
            .removeClass("active");

        this.$tabValidStake.click();
    };

    public cacheDOMElements() {
        super.cacheDOMElements();
        this.$tabSettlement = this.$("#tabSettlement");
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