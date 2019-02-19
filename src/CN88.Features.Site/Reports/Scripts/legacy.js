/// <reference path="../../shared/scripts/core.ts" />
/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
"use strict";
var LegacyReport = /** @class */ (function () {
    function LegacyReport($, dateRangePickerComponent, sportGroupSelectionComponent, betTypeSelectionComponent) {
        var _this = this;
        this.init = function () {
            _this.$.blockUI();
            _this.dateRangePickerComponent.render();
            _this.sportGroupSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-sportgroup", "UserSelectedSportGroupId"));
            _this.betTypeSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-bettype", "UserSelectedBetTypeId"));
            _this.$.unblockUI();
        };
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.sportGroupSelectionComponent = sportGroupSelectionComponent;
        this.betTypeSelectionComponent = betTypeSelectionComponent;
    }
    return LegacyReport;
}());
$(function () {
    var legacyReport = new LegacyReport($, new DateRangePickerComponent($), new SingleSelectionDropdownComponent($), new SingleSelectionDropdownComponent($));
    legacyReport.init();
});
//# sourceMappingURL=legacy.js.map