/// <reference path="../../shared/scripts/components/single-selection-dropdown.ts" />
/// <reference path="../../shared/scripts/components/daterangepicker.ts" />
"use strict";
var ReportIndexView = /** @class */ (function () {
    function ReportIndexView($, dateRangePickerComponent, sportGroupSelectionComponent, betTypeSelectionComponent) {
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
    return ReportIndexView;
}());
$(function () {
    var reportIndexView = new ReportIndexView($, new DateRangePickerComponent($), new SingleSelectionDropdownComponent($), new SingleSelectionDropdownComponent($));
    reportIndexView.init();
});
//# sourceMappingURL=index.js.map