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
        super($, dateRangePickerComponent, sportGroupSelectionComponent, betTypeSelectionComponent, liveStatusSelectionComponent, linkBuilderComponent, pagerComponent)
    }

    init = () => {
        this.$.blockUI();
        this.cacheDOMElements();
        this.initTooltip();

        this.dateRangePickerComponent.render();
        this.sportGroupSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-sportgroup", "UserSelectedSportGroupId"));
        this.betTypeSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-bettype", "UserSelectedBetTypeId"));
        this.liveStatusSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-livestatus", "UserSelectedLiveStatusId"));
        this.linkBuilderComponent.render();

        this.registerEvents();

        let reportType = this.$reportType.val();
        let custLevelId = this.$custLevelId.val();
        if (reportType !== "1" && custLevelId !== "1") {
            this.$tabValidStake.click();
        } else {
            this.$tabSettlement.click();
        }

        this.$.unblockUI();
    };

    public cacheDOMElements() {
        super.cacheDOMElements();
        this.$reportType = this.$("#ReportType");
        this.$custLevelId = this.$("#CustLevelId");
        this.$tabValidStake = this.$("#tabValidStake");
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