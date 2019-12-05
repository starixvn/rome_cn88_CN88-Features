"use strict";

class SecurityCodeChecker {
    $: JQueryStatic;
    top: Window;
    verifySecurityCodeUrl: string;
    needSecurityCode = 999991;
    bootboxWidth = 350;
    forbiddenCode = 403;
    forbiddenAlertTitle: string;
    forbiddenAlertOKLabel: string;

    constructor($: JQueryStatic, top: Window, forbiddenAlertTitle = "", forbiddenAlertOKLabel = "OK") {
        this.$ = $;
        this.top = top;
        this.verifySecurityCodeUrl = $("#VerifySecurityCodeLink").val();
        this.forbiddenAlertTitle = forbiddenAlertTitle;
        this.forbiddenAlertOKLabel = forbiddenAlertOKLabel;
    }

    checkSecurityCode(result, options): void {
        if (result.Code === this.forbiddenCode && result.Message) {
            this.showAlert(result.Message);
            return;
        }

        if (result.code === this.needSecurityCode) {
            const url: string = options.url;
            this.top[url] = () => {
                this.$.ajax({
                    type: "POST",
                    url: options.url,
                    data: options.data,
                    headers: this.setupAntiXSRF(),
                    dataType: "json",
                    success: data => {
                        data.IsSuccess ?
                            options.onComplete(options.onCompleteParams) :
                            this.top["popupManager"].bootboxAlert(options.alertTitle, data.Message, undefined, this.bootboxWidth);
                    }
                });
            };

            this.showSecCodePopup(options.url);
            return;
        }

        options.onComplete(options.onCompleteParams);
    }

    showSecCodePopup(preUrl: string): void {
        this.top["popupManager"].init(this.$, this.top);
        this.top["popupManager"].open({
            title: "",
            url: this.verifySecurityCodeUrl,
            width: 500,
            height: 190,
            preUrl: preUrl
        });
    }

    setupAntiXSRF() {
        const token: string = this.$("input[name=__RequestVerificationToken]").val();
        const headers = {};
        headers["__RequestVerificationToken"] = token;

        return headers;
    }

    showAlert(message: string): void {
        this.top["popupManager"].init(this.$, this.top);
        this.top["popupManager"].bootboxAlert(this.forbiddenAlertTitle, message, null, this.bootboxWidth, this.forbiddenAlertOKLabel);
    }
}