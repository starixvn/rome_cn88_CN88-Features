class PopupBase {
    public $: JQueryStatic;
    public topWindow: any;
    resources = {
        UpdateSuccessfully: '',
        InvalidCredit: '',
        InputNumberError: '',
        ThereIsNoChange: ''
    };

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    };

    public registerEvents() {
        this.$(".btn-cancel").bind("click", this.closePopup);
    };

    public getResource() {
        this.resources.UpdateSuccessfully = this.$("#UpdateSuccessfully").val();
    };

    closePopup = () => {
        this.topWindow.popupManager.close();
    };

    public submit(url: string, data: any) {
        const self = this;
        this.$.blockUI();

        const mainFrame = this.topWindow.main;

        if (typeof mainFrame.customerList !== "undefined") {
            mainFrame.customerList.reloadData();
        }

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
                data.IsSuccess ? self.updateSuccesfully() : self.showFailedMessage(data.Message);
            })
            .fail(function (jqXHR: JQueryXHR) {
                if (jqXHR.status === 403) {
                    self.showFailedMessage(jqXHR.responseJSON.message);
                }
            })
            .always(function () { self.$.unblockUI(); });
    };

    updateSuccesfully() {
        this.showSuccesfullyMessage();

        setTimeout(() => {
            this.closePopup();
        }, 1500);
    };

    showSuccesfullyMessage() {
        this.$("#error-message").html('')
            .text(this.resources.UpdateSuccessfully)
            .removeClass("hidden msg-error")
            .addClass("msg-success");
    };

    public showFailedMessage(message: any) {
        this.$("#error-message").html('')
            .text(message)
            .removeClass("hidden msg-success")
            .addClass("msg-error");
    };
};