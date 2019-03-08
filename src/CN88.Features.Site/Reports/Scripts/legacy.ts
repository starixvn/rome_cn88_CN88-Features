/// <reference path="../../shared/scripts/core.ts" />
/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="../../shared/scripts/components/link-builder.ts" />
/// <reference path="../../shared/scripts/components/client-pager.ts" />
"use strict";
class LegacyReport extends BaseReport {
    $tooltip: JQuery;
    $reportForm: JQuery;
    $exportExcelForm: JQuery;
    $excelIcon: JQuery;
    $ptHistoryUrl: JQuery;
    $FromDate: JQuery;
    $ToDate: JQuery;
    $ptHistory: JQuery;

    linkBuilderComponent: LinkBuilderComponent;
    pagerComponent: ClientPagerComponent;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        betTypeSelectionComponent: SingleSelectionDropdownComponent,
        liveStatusSelectionComponent: SingleSelectionDropdownComponent,
        linkBuilderComponent: LinkBuilderComponent,
        pagerComponent: ClientPagerComponent) {
        super($, dateRangePickerComponent, sportGroupSelectionComponent, betTypeSelectionComponent, liveStatusSelectionComponent)

        this.linkBuilderComponent = linkBuilderComponent;
        this.pagerComponent = pagerComponent;
        this.pagerComponent.pageList = this.pagerComponent.smallPageList;
    }

    init = () => {
        this.$.blockUI();
        this.cacheDOMElements();
        this.initTooltip();

        this.dateRangePickerComponent.render();
        this.sportGroupSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-sportgroup", "UserSelectedSportGroupId", this.onchangeSportType));
        this.betTypeSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-bettype", "UserSelectedBetTypeId"));
        this.liveStatusSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-livestatus", "UserSelectedLiveStatusId"));
        this.linkBuilderComponent.render();
        this.pagerComponent.render();

        this.registerEvents();
        this.$.unblockUI();
    };

    onchangeSportType = () => {
        let sportType = this.sportGroupSelectionComponent.$dropdown.val();
        this.betTypeSelectionComponent.$dropdown.empty();
        for (var i = 0; i < betTypes.length; i++) {
            var betTypeGroup = betTypes[i];
            if (betTypeGroup[0].SportTypeGroup == 0) {
                this.betTypeSelectionComponent.$dropdown.append('<option value=' + betTypeGroup[0].Value + '>' + betTypeGroup[0].Text + '</option>');
            }

            if (betTypeGroup[0].SportTypeGroup == sportType) {
                for (var j = 0; j < betTypeGroup.length; j++) {
                    this.betTypeSelectionComponent.$dropdown.append('<option value=' + betTypeGroup[j].Value + '>' + betTypeGroup[j].Text + '</option>');
                }

                this.betTypeSelectionComponent.$dropdown.multiselect('destroy')
                this.betTypeSelectionComponent.$dropdown.multiselect({
                    multiple: false,
                    maxHeight: 300,
                    numberDisplayed: 1,
                    includeSelectAllOption: false
                });

                break;
            }
        }
    };

    initTooltip = () => {
        this.$tooltip.tooltip();
    }

    public cacheDOMElements() {
        this.$tooltip = this.$('[data-toggle="tooltip"]');
        this.$reportForm = this.$("#report-form");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$excelIcon = this.$("#icon-excel");
        this.$ptHistoryUrl = this.$("#PTHistoryUrl");
        this.$FromDate = this.$("#FromDate");
        this.$ToDate = this.$("#ToDate");
        this.$ptHistory = this.$("#pt-history");
    }

    public registerEvents() {
        this.$reportForm.submit(this.onReportSubmit);
        this.$excelIcon.click(this.onExportExcelEvent)
        this.$(".icon-history").click(this.loadPTHistory);
    }

    onReportSubmit = () => {
        this.sportGroupSelectionComponent.updateSelectedValue();
        this.betTypeSelectionComponent.updateSelectedValue();
        this.liveStatusSelectionComponent.updateSelectedValue();
    };

    onExportExcelEvent = () => {
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    };

    loadPTHistory = (e: JQueryEventObject) => {
        this.$.blockUI();

        let icon = this.$(e.target);
        let custId = icon.data("custid");
        let fromDate = this.$FromDate.val();
        let toDate = this.$ToDate.val();
        var self = this;

        this.$.ajax({
            method: "GET",
            url: this.$ptHistoryUrl.val(),
            cache: false,
            data: { CustId: custId, FromDate: fromDate, ToDate: toDate },
            success: function (result) {
                self.renderPTHistory(result);
                self.showPTHistory(icon.position());
            },
            complete: this.$.unblockUI
        });
    };

    renderPTHistory = (html: string) => {
        if (this.$ptHistory) {
            this.$ptHistory.remove();
        }

        this.$("body").append(html);
        this.$ptHistory = this.$("#pt-history");
    }

    showPTHistory = (position: JQueryCoordinates) => {
        this.$ptHistory.modal({ show: true, backdrop: false });
        this.$ptHistory.css({
            position: "absolute",
            bottom: "auto",
            top: position.top,
            left: position.left - 150
        });

        this.$ptHistory.find('.modal-dialog').css({
            transform: 'none',
            margin: 0,
            width: "100%"
        });
    };
}