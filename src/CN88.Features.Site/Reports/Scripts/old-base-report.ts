/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="../../shared/scripts/components/daterangepicker.ts" />

class OldBaseReport {
    $: JQueryStatic;
    $subMenuItems: JQuery;
    $subMenuWager: JQuery;
    $subMenuLegacy: JQuery;
    $fromDate: JQuery;
    $toDate: JQuery;

    accountingPeriodSelectionComponent: SingleSelectionDropdownComponent;
    dateRangePickerComponent: DateRangePickerComponent;
    sportGroupSelectionComponent: SingleSelectionDropdownComponent;

    constructor(
        $: JQueryStatic,
        accountingPeriodSelectionComponent: SingleSelectionDropdownComponent,
        dateRangePickerComponent: DateRangePickerComponent,
        sportGroupSelectionComponent: SingleSelectionDropdownComponent) {
        this.$ = $;
        this.accountingPeriodSelectionComponent = accountingPeriodSelectionComponent;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.sportGroupSelectionComponent = sportGroupSelectionComponent;
    }

    cacheDOMElements(): void {
        this.$subMenuItems = this.$(".sub-menu-item");
        this.$subMenuWager = this.$("#sub-menu-wager");
        this.$subMenuLegacy = this.$("#sub-menu-legacy");
        this.$fromDate = this.$("#from-date");
        this.$toDate = this.$("#to-date");
    }

    renderComponents(): void {
        this.accountingPeriodSelectionComponent.render(
            new SingleSelectionDropdownOption("dropdown-period", "UserSelectedPeriodId", () => { this.onPeriodChange(); }));
        this.dateRangePickerComponent.render();
        this.sportGroupSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-sportgroup", "UserSelectedSportGroupId"));
    }

    onPeriodChange(): void {
        const $this = this.accountingPeriodSelectionComponent.$dropdown.children("option:selected");
        const fromDate = new Date($this.data("fromdate"));
        const toDate = new Date($this.data("todate"));
        this.$fromDate.val(moment(fromDate).format(cn88Features.format.shortDate_DatePicker));
        this.$toDate.val(moment(toDate).format(cn88Features.format.shortDate_DatePicker));
    }

    reCheckIn(): void {
        this.checkIn();

        const expiredSessionSeconds: number = this.$('#expiredSessionSeconds').val();

        setInterval(() => {
            this.checkIn();
        }, expiredSessionSeconds * 1000);
    }

    checkIn(): void {
        this.$.ajax({ url: this.$("#CheckinURL").val(), type: 'POST' });
    }
}