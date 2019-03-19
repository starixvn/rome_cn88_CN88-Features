/// <reference path="../../shared/scripts/core.ts" />
/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
/// <reference path="../../shared/scripts/components/client-pager.ts" />
/// <reference path="../../shared/scripts/components/link-builder.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
"use strict";
class WagerReport extends LegacyReport {
    $reportType: JQuery;
    $tabValidStake: JQuery;
    $tabSettlement: JQuery;
    $custLevelId: JQuery;

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
        linkBuilderComponent: LinkBuilderComponent,
        pagerComponent: ClientPagerComponent) {
        super($, dateRangePickerComponent, sportGroupSelectionComponent, betTypeSelectionComponent, liveStatusSelectionComponent, linkBuilderComponent, pagerComponent);
    }

    init = () => {
        this.$.blockUI();

        this.cacheDOMElements();
        this.renderComponents();
        this.registerEvents();

        let reportType = this.$reportType.val();
        let custLevelId = this.$custLevelId.val();
        if (reportType !== "1" &&
            custLevelId !== "1" &&
            location.search.indexOf("IsSettlement=True") === -1) {
            this.activateValidStakeTab();
        }
        else if (location.search.indexOf("IsSettlement=True") != -1 ||
            this.$custLevelId.val() === "1" ||
            this.$("IsSettlement").val() === "True" ||
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
        this.$reportType = this.$("#ReportType");
        this.$custLevelId = this.$("#CustLevelId");
        this.$tabValidStake = this.$("#tabValidStake");
        this.$tabSettlement = this.$("#tabSettlement");
        this.elements.tabSelectors = this.$(".tab-selector");
        this.elements.tabContent = this.$(".tab-content");
    }

    public registerEvents() {
        this.$tabSettlement.on("click touchend", () => {
            if (this.$custLevelId.val() !== "1")
                this.$("#IsSettlement").val("True");
        });

        this.$tabValidStake.on("click touchend", () => {
            this.$("#IsSettlement").val("False");
        });

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
    const wagerReport = new WagerReport(
        $,
        new DateRangePickerComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new LinkBuilderComponent($),
        new ClientPagerComponent($));

    wagerReport.init();
})