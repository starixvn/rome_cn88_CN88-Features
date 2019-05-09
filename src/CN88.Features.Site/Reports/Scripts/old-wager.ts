/// <reference path="../../shared/scripts/components/client-pager.ts" />
/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
"use strict";
class OldWagerReport extends OldLegacyReport {
    $tabSettlement: JQuery;
    $tabValidStake: JQuery;
    $custLevelId: JQuery;
    $isSettlement: JQuery;
    isMobile: boolean;
    isOwner: boolean;
    toggleHandler: ToggleHandler;

    constructor(
        $: JQueryStatic,
        accountingPeriodSelectionComponent: SingleSelectionDropdownComponent,
        dateRangePickerComponent: DateRangePickerComponent,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        sortComponent: SortComponent,
        linkBuilderComponent: LinkBuilderComponent,
        pagerComponent: ClientPagerComponent,
        toggleHandler: ToggleHandler) {
        super($, accountingPeriodSelectionComponent, dateRangePickerComponent, sportGroupSelectionComponent, sortComponent, linkBuilderComponent, pagerComponent);
        this.toggleHandler = toggleHandler;
    }

    init(): void {
        this.$.blockUI();

        this.reCheckIn();
        this.cacheDOMElements();

        let reportTable = this.$("#tbl-report");
        this.isMobile = cn88Features.convertAttributeToBool(reportTable.attr("data-isMobile"));
        this.isOwner = cn88Features.convertAttributeToBool(this.$("#IsOwner").val());
        if (this.isMobile) {
            reportTable.stacktable();
        }

        this.renderComponents();
        this.registerEvents();

        this.$subMenuWager.parent().addClass("active");

        if (cn88Features.convertAttributeToBool(this.$isSettlement.val())
            || this.$custLevelId.val() == "1") {
            this.$tabSettlement.click();
        } else {
            this.$tabValidStake.click();
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
        this.$custLevelId = this.$("#CustLevelId");
        this.$tabValidStake = this.$("#tabValidStake");
        this.$tabSettlement = this.$("#tabSettlement");
        this.$isSettlement = this.$("#IsSettlement");
    }

    registerEvents(): void {
        if (this.isMobile) {
            this.$tableReport.find("th").removeClass("sorting");
            this.toggleHandler.render(new ToggleHandlerOption("filter-toggle-icon", "report-form", "tbl-report-stack"));
        }

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

        super.registerEvents();

        if (this.isMobile && this.isOwner) {
            this.$(".btn-collapse-expanse").trigger("click");
        }
    }
}

$(function () {
    const linkBuilderComponent = new LinkBuilderComponent($);
    const oldWagerReport = new OldWagerReport(
        $,
        new SingleSelectionDropdownComponent($),
        new DateRangePickerComponent($),
        new SingleSelectionDropdownComponent($),
        new SortComponent($, linkBuilderComponent),
        linkBuilderComponent,
        new ClientPagerComponent($),
        new ToggleHandler($));

    oldWagerReport.init();
})