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

    public cacheDOMElements() {
        this.$subMenuItems = this.$(".sub-menu-item");
        this.$subMenuWager = this.$("#sub-menu-wager");
        this.$subMenuLegacy = this.$("#sub-menu-legacy");
        this.$fromDate = this.$("#from-date");
        this.$toDate = this.$("#to-date");
    }

    onPeriodChange = () => {
        let $this = this.accountingPeriodSelectionComponent.$dropdown.children("option:selected");
        let fromDate = new Date($this.data("fromdate"));
        let toDate = new Date($this.data("todate"));
        this.$fromDate.val(moment(fromDate).format(cn88Features.format.shortDate_DatePicker));
        this.$toDate.val(moment(toDate).format(cn88Features.format.shortDate_DatePicker));
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