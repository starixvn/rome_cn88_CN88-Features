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
}