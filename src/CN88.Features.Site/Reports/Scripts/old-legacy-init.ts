"use strict";

$(function () {
    const linkBuilderComponent = new LinkBuilderComponent($);
    const oldLegacyReport = new OldLegacyReport(
        $,
        new SingleSelectionDropdownComponent($),
        new DateRangePickerComponent($),
        new SingleSelectionDropdownComponent($),
        new SortComponent($, linkBuilderComponent),
        linkBuilderComponent,
        new ClientPagerComponent($));

    oldLegacyReport.init();
})