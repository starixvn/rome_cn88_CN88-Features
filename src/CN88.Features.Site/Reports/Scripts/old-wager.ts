"use strict";

/// <reference path="old-legacy.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="../../shared/scripts/components/filter-toggle.ts" />
/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
/// <reference path="../../shared/scripts/components/sort.ts" />
/// <reference path="../../shared/scripts/components/link-builder.ts" />
/// <reference path="../../shared/scripts/components/client-pager.ts" />


class OldWagerReport extends OldLegacyReport {
    $tabSettlement: JQuery;
    $tabValidStake: JQuery;
    $isSettlement: JQuery;
    isOwner: boolean;
    toggleHandler: ToggleHandler;
    $nowDataRow: JQuery;

    totalRecords: number;

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
        this.initMobile();

        this.cacheDOMElements();
        this.renderComponents();
        this.registerEvents();

        this.$subMenuWager.parent().addClass("active");

        if (cn88Features.convertAttributeToBool(this.$isSettlement.val())
            || this.$custLevelId.val() === "1") {
            this.$tabSettlement.click();
        } else {
            this.$tabValidStake.click();
        }

        this.$.unblockUI();
    }

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

    private initMobile() {
        const reportTable = this.$("#tbl-report");
        this.isOwner = cn88Features.convertAttributeToBool(this.$("#IsOwner").val());
        if (this.isMobile) {
            reportTable.find("th").removeClass("sorting");
            reportTable.stacktable();
            this.$('body').backtop();
            this.toggleHandler.render(new ToggleHandlerOption("filter-toggle-icon", "report-form", "tbl-report-stack"));
        }
    }

    cacheDOMElements(): void {
        super.cacheDOMElements();

        this.$tabValidStake = this.$("#tabValidStake");
        this.$tabSettlement = this.$("#tabSettlement");
        this.$isSettlement = this.$("#IsSettlement");
        this.$nowDataRow = this.$("#no-data-row");
        this.totalRecords = this.$tableReport.data("totalrecords");
    }

    registerEvents(): void {
        this.$tabSettlement.on("click touchend", () => {
            if (this.$custLevelId.val() !== "1") {
                this.$isSettlement.val("True");
            }

            this.$(".tab-selector").removeClass("active");
            this.$tabSettlement.addClass("active");
            this.$tableReport.find(".valid-stake").hide();
            this.$tableReport.find(".settlement").show();
            this.linkBuilderComponent.updateParam("IsSettlement", "True");

            if (this.totalRecords > 0 && this.$custLevelId.val() === "4") {
                this.$("#profit-margin").attr("colspan", this.$tableReport.data("numberofsettlementcolumns"));
            }

            if (this.totalRecords < 1) {
                this.$nowDataRow.attr("colspan", this.$tableReport.data("numberofsettlementcolumns"));
            }
        });

        this.$tabValidStake.on("click touchend", () => {
            this.$isSettlement.val("False");
            this.$(".tab-selector").removeClass("active");
            this.$tabValidStake.addClass("active");
            this.$tableReport.find(".settlement").hide();
            this.$tableReport.find(".valid-stake").show();
            this.linkBuilderComponent.updateParam("IsSettlement", "False");

            if (this.totalRecords > 0 && this.$custLevelId.val() === "4") {
                this.$("#profit-margin").attr("colspan", this.$tableReport.data("numberofvalidstakecolumns"));
            }

            if (this.totalRecords < 1) {
                this.$nowDataRow.attr("colspan", this.$tableReport.data("numberofvalidstakecolumns"));
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
});