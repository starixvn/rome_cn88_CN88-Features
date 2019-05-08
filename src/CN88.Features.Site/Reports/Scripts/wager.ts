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
    $filterToggleIcon: JQuery;
    isMobile: boolean;
    isOwner: boolean;
    $tabMainContent: JQuery;

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

        let reportTable = this.$("#tbl-report");
        this.isMobile = cn88Features.convertAttributeToBool(reportTable.attr("data-isMobile"));
        this.isOwner = cn88Features.convertAttributeToBool(this.$("#IsOwner").val());
        if (this.isMobile) {
            reportTable.stacktable();
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
        this.$filterToggleIcon = this.$("#filter-toggle-icon");
        this.$tabMainContent = this.$("#tab-main-content,#report-pager");
    }

    public registerEvents() {

        if (this.isMobile) {
            this.$tableReport.find("th").removeClass("sorting");

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

        this.$filterToggleIcon.on("click touchend", (e) => {
            e.preventDefault();
            let isCollapsed = this.$filterToggleIcon.hasClass("icon-filter-down");

            if (isCollapsed) {
                this.$reportForm
                    .slideDown(200);
                this.$tabMainContent
                    .slideUp(210);
                this.$filterToggleIcon
                    .removeClass("icon-filter-down")
                    .addClass("icon-filter-up");
            }
            else {
                this.$reportForm.slideUp(200);
                this.$tabMainContent
                    .slideDown(210);
                this.$filterToggleIcon
                    .removeClass("icon-filter-up")
                    .addClass("icon-filter-down");
            }
        });

        if (this.isOwner) {
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
        new ClientPagerComponent($));

    wagerReport.init();
})