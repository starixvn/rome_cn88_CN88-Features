$(function () {
    const legacyReport = new LegacyReport(
        $,
        new DateRangePickerComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new PagerComponent($));

    legacyReport.init();
})