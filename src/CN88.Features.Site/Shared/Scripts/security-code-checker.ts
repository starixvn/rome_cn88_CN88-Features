"use strict";

class SecurityCodeChecker {
    $: JQueryStatic;
    top: any;
    verifySecurityCodeUrl: string;

    constructor($: JQueryStatic, top: any) {
        this.$ = $;
        this.top = top;
        this.verifySecurityCodeUrl = $("#VerifySecurityCodeLink").val();
    }

    checkSecurityCode(result: any, options: any): void {
        if (result.code === 999991) {
            this.top[options.url] = () => {
                this.$.ajax({
                    type: "POST",
                    url: options.url,
                    data: options.data,
                    headers: this.setupAntiXSRF(),
                    dataType: "json",
                    success: (result) => {
                        result.IsSuccess ?
                            options.onComplete(result) :
                            this.top.popupManager.bootboxAlert(options.alertTitle, result.Message, undefined, 350);
                    }
                });
            };

            this.showSecCodePopup(options.url);
            return;
        }

        location.reload(true);
    }

    showSecCodePopup(preUrl: string): void {
        this.top.popupManager.init(this.$, this.top);
        this.top.popupManager.open({
            title: "",
            url: this.verifySecurityCodeUrl,
            width: 500,
            height: 190,
            preUrl: preUrl
        });
    }

    setupAntiXSRF(): any {
        let token: string = this.$("input[name=__RequestVerificationToken]").val();
        let headers: any = {};
        headers["__RequestVerificationToken"] = token;

        return headers;
    }
}