/// <reference path="../../shared/scripts/core.ts" />
/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
"use strict";
class ReportIndexView extends BaseReport {
    $wagerTab: JQuery;
    $cancelledTab: JQuery;
    $reportType: JQuery;
    $createRptForm: JQuery;
    $wagerRptTypeDropdown: JQuery;
    $cancelledRptTypeDropdown: JQuery;

    $choiceBtn: JQuery;
    $todayBtn: JQuery;
    $tomorrowBtn: JQuery;
    $yesterdayBtn: JQuery;
    $fromDate: JQuery;
    $toDate: JQuery;
    $thisWeekBtn: JQuery;
    $lastWeekBtn: JQuery;
    $thisPeriodBtn: JQuery;
    $lastPeriodBtn: JQuery;
    $clearBtn: JQuery;

    today: any;
    yesterday: any;
    tomorrow: any;
    thisWeek = {
        fromDate: null,
        toDate: null
    };
    lastWeek = {
        fromDate: null,
        toDate: null
    };
    thisPeriod = {
        fromDate: null,
        toDate: null
    };
    lastPeriod = {
        fromDate: null,
        toDate: null
    };

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        betTypeSelectionComponent: SingleSelectionDropdownComponent,
        liveStatusSelectionComponent: SingleSelectionDropdownComponent) {
        super($, dateRangePickerComponent, sportGroupSelectionComponent, betTypeSelectionComponent, liveStatusSelectionComponent)
    }

    init = () => {
        this.$.blockUI();

        this.cachedDOMElements();
        this.renderRptTypeDropdown();
        this.dateRangePickerComponent.render();
        this.sportGroupSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-sportgroup", "UserSelectedSportGroupId"));
        this.betTypeSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-bettype", "UserSelectedBetTypeId"));
        this.liveStatusSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-livestatus", "UserSelectedLiveStatusId"));
        this.calculatedDate();

        this.registerEvents();

        this.$.unblockUI();
    };

    cachedDOMElements = () => {
        this.$wagerTab = this.$("#wager-tab");
        this.$cancelledTab = this.$("#cancelled-tab");
        this.$reportType = this.$("#ReportType");
        this.$createRptForm = this.$("#create-rpt-form");
        this.$wagerRptTypeDropdown = this.$("#dropdown-wagerRptType");
        this.$cancelledRptTypeDropdown = this.$("#dropdown-cancelledRptType");

        this.$choiceBtn = this.$(".btn-quick-choice");
        this.$todayBtn = this.$("#btn-today");
        this.$tomorrowBtn = this.$("#btn-tomorrow");
        this.$yesterdayBtn = this.$("#btn-yesterday");
        this.$fromDate = this.$("#from-date");
        this.$toDate = this.$("#to-date");
        this.$thisWeekBtn = this.$("#btn-this-week");
        this.$lastWeekBtn = this.$("#btn-last-week");
        this.$thisPeriodBtn = this.$("#btn-this-period");
        this.$lastPeriodBtn = this.$("#btn-last-period");
        this.$clearBtn = this.$("#btn-clear");
    };

    calculatedDate = () => {
        let dateRangePicker = this.$("#" + this.dateRangePickerComponent.options.selectorId);
        this.today = new Date(dateRangePicker.data("currentdate"));
        this.yesterday = new Date().setDate(this.today.getDate() - 1);
        this.tomorrow = new Date().setDate(this.today.getDate() + 1);

        var first = this.today.getDate() - this.today.getDay() + 1;
        this.thisWeek.fromDate = new Date().setDate(first);
        this.thisWeek.toDate = new Date().setDate(first + 6);
        this.lastWeek.fromDate = new Date().setDate(first - 7);
        this.lastWeek.toDate = new Date().setDate(first - 1);

        this.thisPeriod.fromDate = new Date(this.$("#ThisPeriod_FromDate").val())
        this.thisPeriod.toDate = new Date(this.$("#ThisPeriod_ToDate").val())
        this.lastPeriod.fromDate = new Date(this.$("#LastPeriod_FromDate").val())
        this.lastPeriod.toDate = new Date(this.$("#LastPeriod_ToDate").val())
    }

    renderRptTypeDropdown = () => {
        this.$wagerRptTypeDropdown.multiselect({
            multiple: false,
            maxHeight: 300,
            numberDisplayed: 1,
            includeSelectAllOption: false,
        });

        this.$cancelledRptTypeDropdown.multiselect({
            multiple: false,
            maxHeight: 300,
            numberDisplayed: 1,
            includeSelectAllOption: false,
        });
    };

    registerEvents = () => {
        this.$createRptForm.submit(() => this.onSubmit());

        this.$todayBtn.click((e) => {
            this.changeDateRange(e, this.today, this.today);
        });

        this.$yesterdayBtn.click((e) => {
            this.changeDateRange(e, this.yesterday, this.yesterday);
        });

        this.$tomorrowBtn.click((e) => {
            this.changeDateRange(e, this.tomorrow, this.tomorrow);
        });

        this.$thisWeekBtn.click((e) => {
            this.changeDateRange(e, this.thisWeek.fromDate, this.thisWeek.toDate);
        });

        this.$lastWeekBtn.click((e) => {
            this.changeDateRange(e, this.lastWeek.fromDate, this.lastWeek.toDate);
        });

        this.$thisPeriodBtn.click((e) => {
            this.changeDateRange(e, this.thisPeriod.fromDate, this.thisPeriod.toDate);
        });

        this.$lastPeriodBtn.click((e) => {
            this.changeDateRange(e, this.lastPeriod.fromDate, this.lastPeriod.toDate);
        });

        this.$clearBtn.click(() => {
            this.$yesterdayBtn.click();
        });
    };

    changeDateRange = (e: JQueryEventObject, fromDate, toDate) => {
        var button = this.$(e.target);
        this.$choiceBtn.removeClass("btn-secondary").addClass("btn-default");
        button.removeClass("btn-default").addClass("btn-secondary");
        this.$fromDate.val(moment(fromDate).format(cn88Features.format.shortDate_DatePicker));
        this.$toDate.val(moment(toDate).format(cn88Features.format.shortDate_DatePicker));
    };

    updateReportTypeValue = () => {
        let selected = [];
        let selectedOptions;
        let this$ = this.$;

        if (this.$wagerTab.hasClass("active")) {
            selectedOptions = this.$wagerRptTypeDropdown.find("option:selected");

            selectedOptions.each(function () {
                selected.push([this$(this).val()]);
            });
        }

        if (this.$cancelledTab.hasClass("active")) {
            selectedOptions = this.$cancelledRptTypeDropdown.find("option:selected");

            selectedOptions.each(function () {
                selected.push([this$(this).val()]);
            });
        }

        this.$reportType.val(selected[0]);
    };

    public onSubmit() {
        this.updateReportTypeValue();
        this.sportGroupSelectionComponent.updateSelectedValue();
        this.betTypeSelectionComponent.updateSelectedValue();
        this.liveStatusSelectionComponent.updateSelectedValue();
    }
}
$(function () {
    const reportIndexView = new ReportIndexView(
        $,
        new DateRangePickerComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($));

    reportIndexView.init();
})