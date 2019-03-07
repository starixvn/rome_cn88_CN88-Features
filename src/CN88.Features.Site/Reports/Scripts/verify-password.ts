"use strict";
class VerifyPassword {
    $: JQueryStatic;
    $txtPassword: JQuery;
    $formVerifyPassword: JQuery;
    $hashKey: JQuery;
    $messageContainer: JQuery;
    resources: any;
    hashKey: string;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.cacheDOMElements();

        this.hashKey = this.$hashKey.val();

        this.registerEvents();
    };

    cacheDOMElements = () => {
        this.resources = {
            passwordCannotEmpty: this.$("#password-cannot-empty").val(),
            passwordDoesNotMeetMinimunRequirement: this.$("#password-does-not-min-requirement").val()
        };

        this.$txtPassword = this.$("#txtPassword");
        this.$formVerifyPassword = this.$("#formVerifyPwd");
        this.$hashKey = this.$("#HashKey");
        this.$messageContainer = this.$("#message-container");
    };

    registerEvents = () => {
        this.$formVerifyPassword.submit(this.onSubmit);
    };

    onSubmit = () => {
        let password: string = this.$txtPassword.val();

        if (password.length === 0) {
            this.showError(this.$messageContainer, this.resources.passwordCannotEmpty);
            this.$txtPassword.focus();
            this.$.unblockUI();
            return false;
        }

        if (!this.isValidPassword(password)) {
            this.showError(this.$messageContainer, this.resources.passwordDoesNotMeetMinimunRequirement);
            this.$txtPassword.focus();
            this.$.unblockUI();
            return false;
        }

        let encryptedPwd = this.encrypt(this.$txtPassword.val(), this.hashKey);
        this.$txtPassword.val(encryptedPwd);

        return true;
    };

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

    showError = ($target: any, error: string) => {
        $target.html("<div class='msg msg-error'>" + error + "</div>");
    };

    encrypt = (plainText: string, secretKey: string) => {
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
}
declare var CryptoJS: any;
$(function () {
    const verifyPwd = new VerifyPassword($);
    verifyPwd.init();
})