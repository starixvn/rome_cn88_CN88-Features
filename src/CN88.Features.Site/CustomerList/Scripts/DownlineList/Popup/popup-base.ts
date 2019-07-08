class PopupBase {
    public $: JQueryStatic;
    public topWindow: Window;

    resources = {
        UpdateSuccessfully: '',
        InvalidCredit: '',
        InputNumberError: '',
        ThereIsNoChange: ''
    };

    constructor($: JQueryStatic, topWindow: Window) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    public registerEvents() {
        this.$(".btn-cancel").bind("click", this.closePopup);
    }

    public getResource() {
        this.resources.UpdateSuccessfully = this.$("#UpdateSuccessfully").val();
    }

    closePopup = () => {
        this.topWindow["popupManager"].close();
    };

    public submit(url: string, data, levelName) {
        this.$.blockUI();
        const unauthorizedCode = 403, mainFrame = this.topWindow["main"];

        if (typeof mainFrame.customerList !== "undefined") {
            mainFrame.customerList.reloadData(levelName);
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
            .done((result) => {
                result.IsSuccess ? this.updateSuccesfully() : this.showFailedMessage(result.Message);
            })
            .fail((jqXHR: JQueryXHR) => {
                if (jqXHR.status === unauthorizedCode) {
                    this.showFailedMessage(jqXHR.responseJSON.message);
                }
            })
            .always(this.$.unblockUI);
    }

    updateSuccesfully() {
        this.showSuccesfullyMessage();
        const timeoutDefault = 2000;

        setTimeout(() => {
            this.closePopup();
        }, timeoutDefault);
    }

    showSuccesfullyMessage() {
        this.$("#error-message").html('')
            .text(this.resources.UpdateSuccessfully)
            .removeClass("hidden msg-error")
            .addClass("msg-success");
    }

    public showFailedMessage(message: string) {
        this.$("#error-message").html('')
            .text(message)
            .removeClass("hidden msg-success")
            .addClass("msg-error");
    }
}