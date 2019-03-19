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
        super($, accountingPeriodSelectionComponent, dateRangePickerComponent, sportGroupSelectionComponent, pagerComponent);
    }

    init(): void {
        this.$.blockUI();

        this.reCheckIn();
        this.cacheDOMElements();
        this.renderComponents();
        this.registerEvents();

        this.$subMenuWager.parent().addClass("active");

        if (location.search.indexOf("IsSettlement=True") != -1 ||
            location.search.indexOf("CustLevelId=1") != -1 ||
            this.$("IsSettlement").val() == "True" ||
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

    cacheDOMElements(): void {
        super.cacheDOMElements();
        this.$tabSettlement = this.$("#tabSettlement");
        this.$tabValidStake = this.$("#tabValidStake");
        this.elements.tabSelectors = $(".tab-selector");
        this.elements.tabContent = $(".tab-content");
    }

    registerEvents(): void {
        let self = this;
        super.registerEvents();

        this.$tabSettlement.on("click touchend", () => {
            if (location.search.indexOf("CustLevelId=1") == -1)
                this.$("#IsSettlement").val("True");
        });

        this.$tabValidStake.on("click touchend", () => {
            this.$("#IsSettlement").val("False");
        });

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