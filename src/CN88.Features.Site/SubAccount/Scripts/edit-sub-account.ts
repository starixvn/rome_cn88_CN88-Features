"use strict";
class EditSubAccount {
    $: JQueryStatic;
    top: any;

    constructor($: JQueryStatic, top: any) {
        this.$ = $;
        this.top = top;
    }

    edit(subAccId: number): void {
        this.submitForm({
            custId: subAccId,
            firstName: this.$("#firstName").val(),
            lastName: this.$("#lastName").val(),
            closed: this.$("#statusClosed").val()
        });
    }

    submitForm(formData: any): void {
        this.$.blockUI();
        let self: any = this;

        let messageContainer = this.$("#message-container");
        const message = new Message();
        message.hideMessage(messageContainer);

        let formSubAccount = this.$("#formSubAccount");
        this.$.ajax({
            type: formSubAccount.attr("method"),
            url: formSubAccount.attr("action"),
            headers: this.setupAntiXSRF(),
            data: formData,
            dataType: "json",
            success: function (data: any) {
                if (data.ErrCode) {
                    data = { Code: data.ErrCode, Message: data.ErrMsg };
                }

                message.showMessage(messageContainer, new Result(data));
            },
            error: function (xhr: any) {
                if (xhr.status === 401) {
                    alert("Oops... you are here because of a system error, please notify the administrators.");
                    self.top.siteInfrastructure.logout();
                }
            },
            complete: self.$.unblockUI
        });
    }

    setupAntiXSRF(): any {
        let token = this.$("input[name=__RequestVerificationToken]").val();
        let headers = {};
        headers["__RequestVerificationToken"] = token;

        return headers;
    }
}