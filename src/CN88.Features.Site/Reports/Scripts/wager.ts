/// <reference path="../../shared/scripts/core.ts" />
/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
/// <reference path="../../shared/scripts/components/client-pager.ts" />
/// <reference path="../../shared/scripts/components/link-builder.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
"use strict";
class WagerReport extends LegacyReport {
    $tabValidStake: JQuery;
    $tabSettlement: JQuery;
    $custLevelId: JQuery;
    $tableContent: JQuery;
    $isSettlement: JQuery;
    isMobile: boolean;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        betTypeSelectionComponent: SingleSelectionDropdownComponent,
        liveStatusSelectionComponent: SingleSelectionDropdownComponent,
        sortComponent: SortComponent,
        linkBuilderComponent: LinkBuilderComponent,
        pagerComponent: ClientPagerComponent) {
        super($, dateRangePickerComponent, sportGroupSelectionComponent, betTypeSelectionComponent, liveStatusSelectionComponent, sortComponent, linkBuilderComponent, pagerComponent);
    }

    init = () => {
        this.$.blockUI();

        this.isMobile = cn88Features.convertAttributeToBool(this.$("#tbl-report").attr("data-isMobile"));
        if (this.isMobile) {
            this.$("#tbl-report").stacktable();
        }

        this.cacheDOMElements();
        this.renderComponents();
        this.registerEvents();

        if (!this.isMobile) {
            if (cn88Features.convertAttributeToBool(this.$isSettlement.val())
                || this.$custLevelId.val() == "1") {
                this.$tabSettlement.click();
            } else {
                this.$tabValidStake.click();
            }
        }

        this.$.unblockUI();
    };

    public cacheDOMElements() {
        super.cacheDOMElements();
        this.$custLevelId = this.$("#CustLevelId");
        this.$tabValidStake = this.$("#tabValidStake");
        this.$tabSettlement = this.$("#tabSettlement");
        this.$isSettlement = this.$("#IsSettlement");
    }

    public registerEvents() {

        if (!this.isMobile) {
            this.$tabSettlement.on("click touchend", () => {
                if (this.$custLevelId.val() !== "1") {
                    this.$isSettlement.val("True");
                }

                this.$(".tab-selector").removeClass("active")
                this.$tabSettlement.addClass("active");
                this.$tableReport.find(".valid-stake").hide();
                this.$tableReport.find(".settlement").show();
                this.linkBuilderComponent.updateParam("IsSettlement", "True");
            });

            this.$tabValidStake.on("click touchend", () => {
                this.$isSettlement.val("False");
                this.$(".tab-selector").removeClass("active")
                this.$tabValidStake.addClass("active");
                this.$tableReport.find(".settlement").hide();
                this.$tableReport.find(".valid-stake").show();
                this.linkBuilderComponent.updateParam("IsSettlement", "False");

                if (this.$custLevelId.val() == "4") {
                    this.$("#profit-margin").attr("colspan", 8);
                }
            });
        }

        super.registerEvents();
    }
}

$(function () {
    const linkBuilderComponent = new LinkBuilderComponent($);
    const wagerReport = new WagerReport(
        $,
        new DateRangePickerComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new SortComponent($, linkBuilderComponent),
        linkBuilderComponent,
        new ClientPagerComponent($));

    wagerReport.init();
})