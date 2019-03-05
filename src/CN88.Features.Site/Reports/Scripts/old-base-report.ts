class OldBaseReport {
    $: JQueryStatic;

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
}