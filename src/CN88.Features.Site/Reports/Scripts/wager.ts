/// <reference path="../../shared/scripts/core.ts" />
/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
/// <reference path="../../shared/scripts/components/client-pager.ts" />
/// <reference path="../../shared/scripts/components/link-builder.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
"use strict";
class WagerReport extends LegacyReport {
    $tabValidStake: JQuery;
    $tabSettlement: JQuery;
    $tableContent: JQuery;
    $isSettlement: JQuery;
    isOwner: boolean;
    $tabMainContent: JQuery;
    toggleHandler: ToggleHandler;
    $nowDataRow: JQuery;

    totalRecords: number;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        betTypeSelectionComponent: SingleSelectionDropdownComponent,
        liveStatusSelectionComponent: SingleSelectionDropdownComponent,
        sortComponent: SortComponent,
        linkBuilderComponent: LinkBuilderComponent,
        pagerComponent: ClientPagerComponent,
        toggleHandler: ToggleHandler) {
        super($, dateRangePickerComponent, sportGroupSelectionComponent, betTypeSelectionComponent, liveStatusSelectionComponent, sortComponent, linkBuilderComponent, pagerComponent);
        this.toggleHandler = toggleHandler;
    }

    init = () => {
        this.$.blockUI();

        this.initForMobile();
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

    initForMobile = () => {
        let reportTable = this.$("#tbl-report");
        this.isMobile = cn88Features.convertAttributeToBool(reportTable.attr("data-isMobile"));
        this.isOwner = cn88Features.convertAttributeToBool(this.$("#IsOwner").val());
        if (this.isMobile) {
            reportTable.stacktable();
            this.$('body').backtop();
        }
    };

    public cacheDOMElements() {
        super.cacheDOMElements();

        this.$tabValidStake = this.$("#tabValidStake");
        this.$tabSettlement = this.$("#tabSettlement");
        this.$isSettlement = this.$("#IsSettlement");
        this.$tabMainContent = this.$("#tab-main-content,#report-pager");
        this.$nowDataRow = this.$("#no-data-row");
        this.totalRecords = this.$tableReport.data("totalrecords");
    }

    public registerEvents() {
        if (this.isMobile) {
            this.$tableReport.find("th").removeClass("sorting");
            this.toggleHandler.render(new ToggleHandlerOption("filter-toggle-icon", "report-form", "tbl-report-stack"));
        }
        else {
            this.$tabSettlement.on("click touchend", () => {
                if (this.$custLevelId.val() !== "1") {
                    this.$isSettlement.val("True");
                }

                this.$(".tab-selector").removeClass("active")
                this.$tabSettlement.addClass("active");
                this.$tableReport.find(".valid-stake").hide();
                this.$tableReport.find(".settlement").show();
                this.linkBuilderComponent.updateParam("IsSettlement", "True");

            if (this.totalRecords > 0 && this.$custLevelId.val() == "4") {
                this.$("#profit-margin").attr("colspan", this.$tableReport.data("numberofsettlementcolumns"));
            }

            if (this.totalRecords < 1) {
                this.$nowDataRow.attr("colspan", this.$tableReport.data("numberofsettlementcolumns"))
            }
            });

            this.$tabValidStake.on("click touchend", () => {
                this.$isSettlement.val("False");
                this.$(".tab-selector").removeClass("active")
                this.$tabValidStake.addClass("active");
                this.$tableReport.find(".settlement").hide();
                this.$tableReport.find(".valid-stake").show();
                this.linkBuilderComponent.updateParam("IsSettlement", "False");

            if (this.totalRecords > 0 && this.$custLevelId.val() == "4") {
                this.$("#profit-margin").attr("colspan", this.$tableReport.data("numberofvalidstakecolumns"));
            }

            if (this.totalRecords < 1) {
                this.$nowDataRow.attr("colspan", this.$tableReport.data("numberofvalidstakecolumns"))
                }
            });
        }

        super.registerEvents();

        if (this.isMobile && this.isOwner) {
            this.$(".btn-collapse-expanse").trigger("click");
        }
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
        new ClientPagerComponent($),
        new ToggleHandler($));

    wagerReport.init();
})