$(function () {
    const oldLegacyReport = new OldLegacyReport(
        $,
        new SingleSelectionDropdownComponent($),
        new DateRangePickerComponent($),
        new SingleSelectionDropdownComponent($),
        new ClientPagerComponent($));

    oldLegacyReport.init();
})