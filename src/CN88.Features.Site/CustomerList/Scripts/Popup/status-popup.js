var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var StatusPopup = /** @class */ (function (_super) {
    __extends(StatusPopup, _super);
    function StatusPopup($, topWindow) {
        var _this = _super.call(this, $, topWindow) || this;
        _this.init = function () {
            _this.registerEvents();
            _this.getResource();
        };
        _this.updateStatus = function () {
            var custId = _this.$("#CustId").val(), updateStatusId = _this.$("#dropdown-status").val(), data = { "CustId": custId, "UpdateStatusId": updateStatusId };
            if (_this.$("#CurrentStatusId").val() === updateStatusId) {
                _this.showFailedMessage(_this.resources.ThereIsNoChange);
                return;
            }
            _this.submit("UpdateCustomerStatus", data, custId, updateStatusId);
        };
        return _this;
    }
    StatusPopup.prototype.registerEvents = function () {
        _super.prototype.registerEvents.call(this);
        this.$(".btn-submit").bind("click", this.updateStatus);
    };
    StatusPopup.prototype.getResource = function () {
        _super.prototype.getResource.call(this);
        this.resources.ThereIsNoChange = this.$("#ThereIsNoChange").val();
    };
    StatusPopup.prototype.reloadCallback = function (custId, newValue) {
        var mainFrame = this.topWindow.main;
        var menuFrame = this.topWindow.menu;
        if (typeof menuFrame.reloadStatusValue === "function") {
            menuFrame.reloadStatusValue(custId, newValue);
        }
        if (typeof mainFrame.customerList !== "undefined" && typeof mainFrame.customerList.reloadStatusValue === "function") {
            mainFrame.customerList.reloadStatusValue(custId, newValue);
        }
    };
    return StatusPopup;
}(PopupBase));
;
$(function () {
    var statusPopup = new StatusPopup($, window.top);
    statusPopup.init();
});
//# sourceMappingURL=status-popup.js.map