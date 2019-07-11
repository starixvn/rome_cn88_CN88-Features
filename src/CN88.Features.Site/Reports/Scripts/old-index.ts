/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
/// <reference path="old-base-report.ts" />

class OldReportIndexView extends OldBaseReport {
    $displayMode: JQuery;
    $reportType: JQuery;
    $createRptForm: JQuery;
    $wagerRptTypeDropdown: JQuery;
    $clearBtn: JQuery;

    fromDate: number;
    toDate: number;

    constructor(
        $: JQueryStatic,
        accountingPeriodSelectionComponent: SingleSelectionDropdownComponent,
        dateRangePickerComponent: DateRangePickerComponent,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent) {
        super($, accountingPeriodSelectionComponent, dateRangePickerComponent, sportGroupSelectionComponent);
    }

    init(): void {
        this.$.blockUI();

        this.reCheckIn();

        this.cachedDOMElements();
        this.renderComponents();
        this.registerEvents();

        this.fromDate = this.$fromDate.val();
        this.toDate = this.$toDate.val();

        const displayMode = this.$displayMode.val();
        this.$subMenuItems.removeClass("active");
        if (displayMode === "1") {
            this.$subMenuWager.parent().addClass("active");
        } else {
            this.$subMenuLegacy.parent().addClass("active");
        }

        this.$.unblockUI();
    }

    cachedDOMElements(): void {
        super.cacheDOMElements();
        this.$displayMode = this.$("#DisplayMode");
        this.$reportType = this.$("#ReportType");
        this.$createRptForm = this.$("#create-rpt-form");
        this.$wagerRptTypeDropdown = this.$("#dropdown-wagerRptType");
        this.$clearBtn = this.$("#btn-clear");
    }

    renderComponents(): void {
        this.renderRptTypeDropdown();
        super.renderComponents();
    }

    renderRptTypeDropdown(): void {
        this.$wagerRptTypeDropdown.multiselect({
            multiple: false,
            maxHeight: 300,
            numberDisplayed: 1,
            includeSelectAllOption: false,
        });
    }

    registerEvents = () => {
        this.$createRptForm.submit(this.onSubmit);
        this.$clearBtn.click(this.reset);
    }

    onSubmit = () => {
        const reportType = this.$wagerRptTypeDropdown.children("option:selected").val();
        this.$reportType.val(reportType);
        this.accountingPeriodSelectionComponent.updateSelectedValue();
        this.sportGroupSelectionComponent.updateSelectedValue();
    };

    reset = () => {
        this.resetReportType();
        this.resetAccountingSheet();
        this.resetDatePicker();
        this.sportGroupSelectionComponent.changeSelection(0);
    };

    resetReportType(): void {
        (this.$wagerRptTypeDropdown[0] as HTMLSelectElement).selectedIndex = 0;
        this.$wagerRptTypeDropdown.multiselect("refresh");
    }

    resetAccountingSheet(): void {
        (this.accountingPeriodSelectionComponent.$dropdown[0] as HTMLSelectElement).selectedIndex = 0;
        this.accountingPeriodSelectionComponent.$dropdown.multiselect("refresh");
    }

    resetDatePicker(): void {
        this.$fromDate.val(this.fromDate);
        this.$toDate.val(this.toDate);
    }
}

$(function () {
    const oldReportIndexView = new OldReportIndexView(
        $,
        new SingleSelectionDropdownComponent($),
        new DateRangePickerComponent($),
        new SingleSelectionDropdownComponent($));

    oldReportIndexView.init();
});