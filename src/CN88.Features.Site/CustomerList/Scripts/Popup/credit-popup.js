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
var CreditPopup = /** @class */ (function (_super) {
    __extends(CreditPopup, _super);
    function CreditPopup($, topWindow) {
        var _this = _super.call(this, $, topWindow) || this;
        _this.init = function () {
            _this.registerEvents();
            _this.getResource();
            _this.cleaveCredit = new Cleave('#credit', {
                numeral: true,
                numeralPositiveOnly: true,
                numeralDecimalMark: '.',
                delimiter: ','
            });
        };
        _this.updateCredit = function () {
            var minCredit = parseFloat(_this.$("#MinCredit").val()), maxCredit = parseFloat(_this.$("#MaxCredit").val()), credit = parseFloat(_this.cleaveCredit.getRawValue()), custId = _this.$("#CustId").val(), custLevelId = _this.$("#CustLevelId").val(), data = {
                "CustId": custId,
                "CustLevelId": custLevelId,
                "CreditAmount": credit
            };
            if (isNaN(credit)) {
                _this.showFailedMessage(_this.resources.InputNumberError);
                return;
            }
            if (credit < minCredit || credit > maxCredit) {
                _this.showFailedMessage(_this.resources.ExceededAgentCredit);
                return;
            }
            _this.submit("UpdateCredit", data, custId, credit);
        };
        return _this;
    }
    CreditPopup.prototype.registerEvents = function () {
        _super.prototype.registerEvents.call(this);
        this.$(".btn-submit").bind("click", this.updateCredit);
    };
    CreditPopup.prototype.getResource = function () {
        _super.prototype.getResource.call(this);
        this.resources.ExceededAgentCredit = this.$("#ExceededAgentCredit").val();
        this.resources.InputNumberError = this.$("#InputNumberError").val();
    };
    CreditPopup.prototype.reloadCallback = function (custId, newValue) {
        this.topWindow.main.customerList.reloadCreditValue(custId, newValue);
    };
    return CreditPopup;
}(PopupBase));
$(function () {
    var creditPopup = new CreditPopup($, window.top);
    creditPopup.init();
});
//# sourceMappingURL=credit-popup.js.map