/// <reference path="../../shared/scripts/core.ts" />
/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
"use strict";
var betTypes: any = betTypes;
let NexDatePickerHelper: any;
class ReportIndexView extends BaseReport {
    $wagerTab: JQuery;
    $cancelledTab: JQuery;
    $reportType: JQuery;
    $createRptForm: JQuery;
    $wagerRptTypeDropdown: JQuery;
    $cancelledRptTypeDropdown: JQuery;

    $dateRangePicker: JQuery;
    $fromDateContainer: JQuery;
    $toDateContainer: JQuery;

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

    minDate: Date;
    thisPeriod = {
        fromDate: null,
        toDate: null
    };
    lastPeriod = {
        fromDate: null,
        toDate: null
    };

    defaultFromDate: Date;
    defaultToDate: Date;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent,
        betTypeSelectionComponent: SingleSelectionDropdownComponent,
        liveStatusSelectionComponent: SingleSelectionDropdownComponent) {
        super($, dateRangePickerComponent, sportGroupSelectionComponent, betTypeSelectionComponent, liveStatusSelectionComponent);
    }

    init = () => {
        this.$.blockUI();

        this.cachedDOMElements();
        this.renderComponents();
        this.calculatedDate();
        this.registerEvents();

        this.$.unblockUI();
    };

    renderComponents(): void {
        this.renderRptTypeDropdown();
        super.renderComponents();
    }

    cachedDOMElements(): void {
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

        this.$dateRangePicker = this.$("#" + this.dateRangePickerComponent.options.selectorId);
        this.$fromDateContainer = this.$("#from-date-container");
        this.$toDateContainer = this.$("#to-date-container");

        
    };

    calculatedDate(): void {
        this.minDate = new Date(this.$dateRangePicker.data("mindate"));
        this.thisPeriod.fromDate = new Date(this.$("#ThisPeriod_FromDate").val());
        this.thisPeriod.toDate = new Date(this.$("#ThisPeriod_ToDate").val());
        this.lastPeriod.fromDate = new Date(this.$("#LastPeriod_FromDate").val());
        this.lastPeriod.toDate = new Date(this.$("#LastPeriod_ToDate").val());
        this.defaultFromDate = new Date(this.$fromDate.val());
        this.defaultToDate = new Date(this.$toDate.val());
    }

    renderRptTypeDropdown(): void {
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

    registerEvents(): void {
        this.$createRptForm.submit(() => this.onSubmit());

        this.$todayBtn.click((e) => {
            this.onQuickChoiceClick(e, this.getToday(), this.getToday());
        });

        this.$yesterdayBtn.click((e) => {
            let yesterday: Date;

            if (this.$fromDate.val() === this.$toDate.val()) {
                yesterday = this.getYesterday(new Date(this.$fromDate.val()));
            } else {
                yesterday = this.getYesterday(this.getToday());
            }

            if (yesterday < this.minDate) {
                return;
            }

            this.onQuickChoiceClick(e, yesterday, yesterday);
        });

        this.$tomorrowBtn.click((e) => {
            let tomorrow: Date;
            if (this.$fromDate.val() === this.$toDate.val()) {
                tomorrow = this.getTomorrow(new Date(this.$fromDate.val()));
            } else {
                tomorrow = this.getTomorrow(this.getToday());
            }

            this.onQuickChoiceClick(e, tomorrow, tomorrow);
        });

        this.$thisWeekBtn.click((e) => {
            let dateRange = this.getDateRangeOfCurrentWeek(this.getToday());
            this.onQuickChoiceClick(e, dateRange[0], dateRange[1]);
        });

        this.$lastWeekBtn.click((e) => {
            let dateRange = NexDatePickerHelper.getDateRangeOfLastWeek(this.getToday());
            this.onQuickChoiceClick(e, dateRange[0], dateRange[1]);
        });

        this.$thisPeriodBtn.click((e) => {
            this.onQuickChoiceClick(e, this.thisPeriod.fromDate, this.thisPeriod.toDate);
        });

        this.$lastPeriodBtn.click((e) => {
            this.onQuickChoiceClick(e, this.lastPeriod.fromDate, this.lastPeriod.toDate);
        });

        this.$clearBtn.click(() => {
            this.reset();
        });
    };

    getToday(): Date {
        return new Date(this.$dateRangePicker.data("currentdate"));
    }

    getYesterday(date: Date): Date {
        let yesterday = date;
        yesterday.setDate(date.getDate() - 1);
        return yesterday;
    }

    getTomorrow(date: Date): Date {
        let tomorrow = date;
        tomorrow.setDate(date.getDate() + 1);
        return tomorrow;
    }

    getDateRangeOfCurrentWeek(today: Date): [Date, Date] {
        let date = today;
        let baseDate = NexDatePickerHelper.getBaseDateOfWeek(today);
        let startDate = new Date(date.setDate(baseDate));
        let lastDate = new Date(date.setDate(date.getDate() + 6));

        return [startDate, lastDate];
    }

    onQuickChoiceClick(e: JQueryEventObject, fromDate, toDate): void {
        var button = this.$(e.target);
        this.$choiceBtn.removeClass("btn-secondary").addClass("btn-default");
        button.removeClass("btn-default").addClass("btn-secondary");

        this.changeDateRange(fromDate, toDate);
    }

    changeDateRange(fromDate, toDate): void {
        let minDate = new Date(this.$dateRangePicker.data("mindate"));
        let maxDate = new Date(this.$dateRangePicker.data("maxdate"));
        let fDate = NexDatePickerHelper.correctDateInRange(fromDate, minDate, maxDate);
        let tDate = NexDatePickerHelper.correctDateInRange(toDate, minDate, maxDate);

        this.$fromDateContainer.data("DateTimePicker").maxDate(tDate);
        this.$fromDateContainer.data("DateTimePicker").date(fDate);

        this.$toDateContainer.data("DateTimePicker").minDate(fDate);
        this.$toDateContainer.data("DateTimePicker").date(tDate);
    }

    reset(): void {
        this.resetWagerReportType();
        this.resetCancelledReportType();
        this.changeDateRange(this.defaultFromDate, this.defaultToDate);
        this.$choiceBtn.removeClass("btn-secondary").addClass("btn-default");
        this.$yesterdayBtn.addClass("btn-secondary");
        this.sportGroupSelectionComponent.changeSelection(0);
        this.resetBetType();
        this.liveStatusSelectionComponent.changeSelection(2);
    }

    resetWagerReportType(): void {
        (<HTMLSelectElement>this.$wagerRptTypeDropdown[0]).selectedIndex = 0;
        this.$wagerRptTypeDropdown.multiselect('refresh');
    }

    resetCancelledReportType(): void {
        (<HTMLSelectElement>this.$cancelledRptTypeDropdown[0]).selectedIndex = 0;
        this.$cancelledRptTypeDropdown.multiselect('refresh');
    }

    resetBetType(): void {
        this.betTypeSelectionComponent.$dropdown.empty();
        for (var i = 0; i < betTypes.length; i++) {
            var betTypeGroup = betTypes[i];
            if (betTypeGroup[0].SportTypeGroup == 0) {
                this.betTypeSelectionComponent.$dropdown.append('<option value=' + betTypeGroup[0].Value + '>' + betTypeGroup[0].Text + '</option>');
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
    }

    onSubmit = () => {
        this.updateReportTypeValue();
        this.sportGroupSelectionComponent.updateSelectedValue();
        this.betTypeSelectionComponent.updateSelectedValue();
        this.liveStatusSelectionComponent.updateSelectedValue();
    }

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