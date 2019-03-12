class OldBaseReport {
    $: JQueryStatic;
    $subMenuItems: JQuery;
    $subMenuWager: JQuery;
    $subMenuLegacy: JQuery;

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

    public cacheDOMElements() {
        this.$subMenuItems = this.$(".sub-menu-item");
        this.$subMenuWager = this.$("#sub-menu-wager");
        this.$subMenuLegacy = this.$("#sub-menu-legacy");
    }

    init = () => {
        this.checkIn();

        let expiredSessionSeconds: number = this.$('#expiredSessionSeconds').val();
        let $this = this;

        setInterval(function () {
            $this.checkIn();
        }, expiredSessionSeconds * 1000);
    }

    checkIn = () => {
        this.$.ajax({ url: '../OldReports/Checkin', type: 'POST' });
    }
}
$(function () {
    const oldBaseReport = new OldBaseReport(
        $,
        new SingleSelectionDropdownComponent($),
        new DateRangePickerComponent($),
        new SingleSelectionDropdownComponent($));

    oldBaseReport.init();
})