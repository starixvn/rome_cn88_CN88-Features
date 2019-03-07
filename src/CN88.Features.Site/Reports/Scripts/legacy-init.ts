$(function () {
    const legacyReport = new LegacyReport(
        $,
        new DateRangePickerComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new LinkBuilderComponent($),
        new ClientPagerComponent($));

    legacyReport.init();
})