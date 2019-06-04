"use strict";
/// <reference path="old-legacy.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
/// <reference path="../../shared/scripts/components/sort.ts" />
/// <reference path="../../shared/scripts/components/client-pager.ts" />
/// <reference path="../../shared/scripts/components/link-builder.ts" />


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