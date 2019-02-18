var PopupBase = /** @class */ (function () {
    function PopupBase($, topWindow) {
        var _this = this;
        this.resources = {
            UpdateSuccessfully: '',
            ExceededAgentCredit: '',
            InputNumberError: '',
            ThereIsNoChange: ''
        };
        this.closePopup = function () {
            _this.topWindow.ageWnd.close();
        };
        this.$ = $;
        this.topWindow = topWindow;
    }
    PopupBase.prototype.registerEvents = function () {
        this.$(".btn-cancel").bind("click", this.closePopup);
    };
    PopupBase.prototype.getResource = function () {
        this.resources.UpdateSuccessfully = this.$("#UpdateSuccessfully").val();
    };
    PopupBase.prototype.reloadCallback = function (custId, newValue) {
    };
    PopupBase.prototype.submit = function (url, data, custId, newValue) {
        var self = this;
        this.$.blockUI();
        this.$.ajax({
            type: "POST",
            url: url,
            data: data,
            dataType: "json",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("__RequestVerificationToken", $("input[name=\"__RequestVerificationToken\"]").val());
            }
        })
            .done(function (data) {
            data.IsSuccess ? self.updateSuccesfully(custId, newValue) : self.showFailedMessage(data.Message);
        })
            .always(function () { self.$.unblockUI(); });
    };
    PopupBase.prototype.updateSuccesfully = function (custId, newValue) {
        var self = this;
        this.showSuccesfullyMessage();
        setTimeout(function () {
            self.reloadCallback(custId, newValue);
            self.closePopup();
        }, 3000);
    };
    PopupBase.prototype.showSuccesfullyMessage = function () {
        this.$("#error-message").html('')
            .text(this.resources.UpdateSuccessfully)
            .removeClass("hidden msg-error")
            .addClass("msg-success");
    };
    PopupBase.prototype.showFailedMessage = function (message) {
        this.$("#error-message").html('')
            .text(message)
            .removeClass("hidden msg-success")
            .addClass("msg-error");
    };
    return PopupBase;
}());
;
//# sourceMappingURL=popup-base.js.map