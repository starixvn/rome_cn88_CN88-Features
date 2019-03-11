"use strict";

interface String {
    encrypt(secretKey: string): string;
}

declare var CryptoJS: any;

String.prototype.encrypt = function (secretKey: string): string {
    let plainText: string = this.toString();
    const passPhrase = "a5s8d2e9c172";
    let key = CryptoJS.enc.Utf8.parse(passPhrase + secretKey);
    let iv = key;

    let pwd = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(plainText), key,
        {
            keySize: 128 / 8,
            iv: iv,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        });

    return pwd.toString();
};

class CreateSubAccount extends EditSubAccount {
    constructor($: JQueryStatic, top: any) {
        super($, top);
    }

    create(): void {
        let messageContainer: JQuery = this.$("#message-container");
        let password = this.getPassword(messageContainer);

        if (password.length === 0) {
            return;
        }

        let hashKey: string = this.$("#hashKey").val();
        this.submitForm({
            userName: this.$("#userName").val(),
            password: password.encrypt(hashKey),
            firstName: this.$("#firstName").val(),
            lastName: this.$("#lastName").val(),
            pageAccess: this.getPageAccess()
        });
    }

    getPassword(messageContainer: JQuery) {
        let $password: JQuery = this.$("#password");
        let passwordValue: string = $password.val();

        if (passwordValue.length === 0) {
            const message = new Message();
            message.showError(messageContainer, this.$("#requiredPasswordError").val());
            $password.focus();
            return "";
        }

        if (!this.isValidPassword(passwordValue)) {
            const message = new Message();
            message.showError(messageContainer, this.$("#passwordWrongRuleError").val());
            $password.focus();
            return "";
        }

        return passwordValue;
    }

    isValidPassword(password: string) {
        let whiteSpace: RegExp = /^\S*$/;
        let whiteSpaceCase: boolean = password.search(whiteSpace) !== -1;

        let regex1: RegExp = /^(?=.{8,100}$).*\d/i;
        let regex2: RegExp = /^(?=.{8,100}$).*[a-z]/;
        let regex3: RegExp = /^(?=.{8,100}$).*[A-Z]/;
        let regex4: RegExp = /^(?=.{8,100}$).*[\[\]\^\$\.\|\?\*\+\(\)\\~`\!@#%&\-_+={}'""<>:;,]/i;

        let case1: number = password.search(regex1) !== -1 ? 1 : 0;
        let case2: number = password.search(regex2) !== -1 ? 1 : 0;
        let case3: number = password.search(regex3) !== -1 ? 1 : 0;
        let case4: number = password.search(regex4) !== -1 ? 1 : 0;

        return (case1 + case2 + case3 + case4 >= 3) && whiteSpaceCase;
    }

    getPageAccess() {
        let pageAccess = this.getPermissionValue("#totalBets");
        let acManagementEditView = this.getPermissionValue("#acManagementEditView");

        if (acManagementEditView.length > 0) {
            pageAccess += `,${acManagementEditView}`;
        } else {
            let acManagementViewOnly = this.getPermissionValue("#acManagementViewOnly");

            if (acManagementViewOnly.length > 0) {
                pageAccess += `,${acManagementViewOnly}`;   
            }
        }

        let reports = this.getPermissionValue("#reports");

        return reports.length > 0
            ? pageAccess += `,${reports}`
            : pageAccess;
    }

    getPermissionValue(selector: string): string {
        let element: JQuery = this.$(selector);

        return element.is(":checked") ? element.val() : "";
    }
}